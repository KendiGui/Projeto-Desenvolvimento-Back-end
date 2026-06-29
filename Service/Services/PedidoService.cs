using Core.Data;
using Domain.Contracts;
using Domain.Contracts.Exceptions;
using Domain.Contracts.Requests;
using Domain.Contracts.Responses;
using Domain.Entities;
using Domain.Enums;
using Domain.Helpers;
using Domain.Repositories;
using Service.Interfaces;

namespace Service.Services
{
    public class PedidoService(
        IPedidoRepository pedidoRepository,
        IUnidadeRepository unidadeRepository,
        IProdutoRepository produtoRepository,
        IUnidadeProdutoRepository unidadeProdutoRepository,
        IEstoqueRepository estoqueRepository,
        IMovimentoEstoqueRepository movimentoEstoqueRepository,
        IPagamentoRepository pagamentoRepository,
        IPaymentGateway paymentGateway,
        IFidelidadeService fidelidadeService,
        IAuditoriaService auditoriaService,
        IUnitOfWork unitOfWork) : IPedidoService
    {
        private static readonly Dictionary<StatusPedidoEnum, StatusPedidoEnum[]> Transicoes = new()
        {
            [StatusPedidoEnum.Criado] = [StatusPedidoEnum.AguardandoPagamento],
            [StatusPedidoEnum.AguardandoPagamento] = [StatusPedidoEnum.Pago, StatusPedidoEnum.PagamentoRecusado, StatusPedidoEnum.Cancelado],
            [StatusPedidoEnum.Pago] = [StatusPedidoEnum.EmPreparo, StatusPedidoEnum.Cancelado],
            [StatusPedidoEnum.EmPreparo] = [StatusPedidoEnum.Pronto],
            [StatusPedidoEnum.Pronto] = [StatusPedidoEnum.Entregue],
        };

        public async Task<PedidoResponse> CriarPedido(CriarPedidoRequest request, long clienteId, string? ip)
        {
            if (string.IsNullOrWhiteSpace(request.CanalPedido))
                throw new BusinessException("CANAL_PEDIDO_INVALIDO", "canalPedido é obrigatório.", 422, [new ErroDetalhe("canalPedido", "Campo obrigatório")]);

            if (!EnumExtensions.TryParseApi<CanalPedidoEnum>(request.CanalPedido, out var canal))
                throw new BusinessException("CANAL_PEDIDO_INVALIDO", "canalPedido inválido.", 422, [new ErroDetalhe("canalPedido", "Valores aceitos: APP, TOTEM, BALCAO, PICKUP, WEB")]);

            var itensRequest = request.Itens?.ToList() ?? [];
            if (itensRequest.Count == 0)
                throw new ValidationException("O pedido deve conter ao menos um item.", [new ErroDetalhe("itens", "Lista vazia")]);

            var unidade = await unidadeRepository.GetByIdAsync(request.UnidadeId);
            if (unidade is null) throw new NotFoundException("Unidade não encontrada");

            var linhas = new List<LinhaPedido>();
            var faltaEstoque = new List<ErroDetalhe>();

            for (int i = 0; i < itensRequest.Count; i++)
            {
                var item = itensRequest[i];

                if (item.Quantidade <= 0)
                    throw new ValidationException("Quantidade deve ser maior que zero.", [new ErroDetalhe($"itens[{i}].quantidade", "Deve ser maior que zero")]);

                var produto = await produtoRepository.GetByIdAsync(item.ProdutoId);
                if (produto is null)
                    throw new NotFoundException($"Produto {item.ProdutoId} não encontrado");

                var cardapio = await unidadeProdutoRepository.GetByUnidadeProdutoAsync(request.UnidadeId, item.ProdutoId);
                if (cardapio is null || !cardapio.Disponivel || !produto.Ativo)
                    throw new ValidationException($"Produto {produto.Nome} não está disponível nesta unidade.", [new ErroDetalhe($"itens[{i}].produtoId", "Produto indisponível na unidade")]);

                var estoque = await estoqueRepository.GetByUnidadeProdutoAsync(request.UnidadeId, item.ProdutoId);
                var disponivel = estoque?.QuantidadeAtual ?? 0;
                if (estoque is null || disponivel < item.Quantidade)
                    faltaEstoque.Add(new ErroDetalhe($"itens[{i}].quantidade", $"Disponível: {disponivel}"));

                var precoUnitario = cardapio.PrecoCustomizado ?? produto.Preco;
                linhas.Add(new LinhaPedido(produto, estoque, item.Quantidade, precoUnitario));
            }

            if (faltaEstoque.Count > 0)
                throw new EstoqueInsuficienteException("Não há quantidade suficiente para um ou mais itens.", faltaEstoque);

            await unitOfWork.BeginTransactionAsync();

            var pedido = new Pedido
            {
                ClienteId = clienteId,
                UnidadeId = request.UnidadeId,
                CanalPedido = canal,
                Status = StatusPedidoEnum.AguardandoPagamento,
                Total = linhas.Sum(l => l.Subtotal),
                Itens = linhas.Select(l => new ItemPedido
                {
                    ProdutoId = l.Produto.Id,
                    Quantidade = l.Quantidade,
                    PrecoUnitario = l.PrecoUnitario,
                    Subtotal = l.Subtotal
                }).ToList()
            };

            await pedidoRepository.AddAsync(pedido);
            await unitOfWork.SaveChangesAsync();

            var pagamentoResult = await paymentGateway.ProcessarPagamentoAsync(pedido, request.FormaPagamento);

            var pagamento = new Pagamento
            {
                PedidoId = pedido.Id,
                Status = pagamentoResult.Status,
                FormaPagamento = (request.FormaPagamento ?? "MOCK").ToUpperInvariant(),
                GatewayTransactionId = pagamentoResult.TransactionId,
                PayloadRequest = pagamentoResult.PayloadRequest,
                PayloadResponse = pagamentoResult.PayloadResponse
            };
            await pagamentoRepository.AddAsync(pagamento);

            if (pagamentoResult.Aprovado)
            {
                foreach (var linha in linhas)
                {
                    linha.Estoque!.QuantidadeAtual -= linha.Quantidade;
                    await estoqueRepository.UpdateAsync(linha.Estoque);

                    await movimentoEstoqueRepository.AddAsync(new MovimentoEstoque
                    {
                        EstoqueId = linha.Estoque.Id,
                        TipoMovimento = TipoMovimentoEnum.Saida,
                        Quantidade = linha.Quantidade,
                        Motivo = $"Baixa pelo pedido #{pedido.Id}",
                        UsuarioId = clienteId
                    });
                }

                pedido.Status = StatusPedidoEnum.Pago;
                await fidelidadeService.AcumularPontosAsync(clienteId, pedido.Id, pedido.Total);
            }
            else
            {
                pedido.Status = StatusPedidoEnum.PagamentoRecusado;
            }

            await pedidoRepository.UpdateAsync(pedido);

            await auditoriaService.RegistrarAsync(
                acao: "CRIAR_PEDIDO",
                entidade: nameof(Pedido),
                entidadeId: pedido.Id,
                usuarioId: clienteId,
                ip: ip,
                depois: new { pedido.Id, status = pedido.Status.ToApiString(), pedido.Total, canal = canal.ToApiString() });

            await unitOfWork.CommitAsync();

            pedido.Pagamento = pagamento;
            return new PedidoResponse
            {
                PedidoId = pedido.Id,
                Status = pedido.Status.ToApiString(),
                CanalPedido = pedido.CanalPedido.ToApiString(),
                UnidadeId = pedido.UnidadeId,
                ClienteId = pedido.ClienteId,
                Total = pedido.Total,
                CreatedAt = pedido.CreatedAt,
                Pagamento = new PagamentoResponse
                {
                    Id = pagamento.Id,
                    PedidoId = pagamento.PedidoId,
                    Status = pagamento.Status.ToApiString(),
                    FormaPagamento = pagamento.FormaPagamento,
                    TransactionId = pagamento.GatewayTransactionId,
                    CreatedAt = pagamento.CreatedAt
                },
                Itens = linhas.Select(l => new ItemPedidoResponse
                {
                    ProdutoId = l.Produto.Id,
                    Nome = l.Produto.Nome,
                    Quantidade = l.Quantidade,
                    PrecoUnitario = l.PrecoUnitario,
                    Subtotal = l.Subtotal
                }).ToList()
            };
        }

        public async Task<ResultPaginado<PedidoResponse>> ListaPedidos(string? canalPedido, string? status, long requesterId, string requesterRole, int pagina = 1, int tamanhoPagina = 10)
        {
            CanalPedidoEnum? canalFiltro = null;
            if (!string.IsNullOrWhiteSpace(canalPedido))
            {
                if (!EnumExtensions.TryParseApi<CanalPedidoEnum>(canalPedido, out var c))
                    throw new BusinessException("CANAL_PEDIDO_INVALIDO", "canalPedido inválido.", 422);
                canalFiltro = c;
            }

            StatusPedidoEnum? statusFiltro = null;
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (!EnumExtensions.TryParseApi<StatusPedidoEnum>(status, out var s))
                    throw new ValidationException("status inválido.");
                statusFiltro = s;
            }

            long? clienteFiltro = requesterRole == Roles.Cliente ? requesterId : null;

            var pedidos = await pedidoRepository.ListFiltradoAsync(clienteFiltro, canalFiltro, statusFiltro, pagina, tamanhoPagina);

            return pedidos.Map(p => new PedidoResponse
            {
                PedidoId = p.Id,
                Status = p.Status.ToApiString(),
                CanalPedido = p.CanalPedido.ToApiString(),
                UnidadeId = p.UnidadeId,
                ClienteId = p.ClienteId,
                Total = p.Total,
                CreatedAt = p.CreatedAt,
                Pagamento = p.Pagamento is null ? null : new PagamentoResponse
                {
                    Id = p.Pagamento.Id,
                    PedidoId = p.Pagamento.PedidoId,
                    Status = p.Pagamento.Status.ToApiString(),
                    FormaPagamento = p.Pagamento.FormaPagamento,
                    TransactionId = p.Pagamento.GatewayTransactionId,
                    CreatedAt = p.Pagamento.CreatedAt
                },
                Itens = p.Itens.Select(i => new ItemPedidoResponse
                {
                    ProdutoId = i.ProdutoId,
                    Nome = i.Produto?.Nome ?? string.Empty,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario,
                    Subtotal = i.Subtotal
                }).ToList()
            });
        }

        public async Task<PedidoResponse> GetPedido(long pedidoId, long requesterId, string requesterRole)
        {
            var pedido = await pedidoRepository.GetCompletoAsync(pedidoId);
            if (pedido is null) throw new NotFoundException("Pedido não encontrado");

            if (requesterRole == Roles.Cliente && pedido.ClienteId != requesterId)
                throw new ForbiddenException("Você não tem acesso a este pedido.");

            return new PedidoResponse
            {
                PedidoId = pedido.Id,
                Status = pedido.Status.ToApiString(),
                CanalPedido = pedido.CanalPedido.ToApiString(),
                UnidadeId = pedido.UnidadeId,
                ClienteId = pedido.ClienteId,
                Total = pedido.Total,
                CreatedAt = pedido.CreatedAt,
                Pagamento = pedido.Pagamento is null ? null : new PagamentoResponse
                {
                    Id = pedido.Pagamento.Id,
                    PedidoId = pedido.Pagamento.PedidoId,
                    Status = pedido.Pagamento.Status.ToApiString(),
                    FormaPagamento = pedido.Pagamento.FormaPagamento,
                    TransactionId = pedido.Pagamento.GatewayTransactionId,
                    CreatedAt = pedido.Pagamento.CreatedAt
                },
                Itens = pedido.Itens.Select(i => new ItemPedidoResponse
                {
                    ProdutoId = i.ProdutoId,
                    Nome = i.Produto?.Nome ?? string.Empty,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario,
                    Subtotal = i.Subtotal
                }).ToList()
            };
        }

        public async Task<PedidoResponse> AtualizarStatus(long pedidoId, AtualizarStatusPedidoRequest request, long usuarioId, string requesterRole, string? ip)
        {
            if (!EnumExtensions.TryParseApi<StatusPedidoEnum>(request.NovoStatus, out var novoStatus))
                throw new ValidationException("Status informado é inválido.",
                    [new ErroDetalhe("novoStatus", "Valor inválido")]);

            var pedido = await pedidoRepository.GetCompletoAsync(pedidoId);
            if (pedido is null) throw new NotFoundException("Pedido não encontrado");

            // Valida a transição de status.
            if (pedido.Status == novoStatus)
                throw new StatusPedidoInvalidoException($"O pedido já está no status {novoStatus.ToApiString()}.");

            if (!Transicoes.TryGetValue(pedido.Status, out var permitidos) || !permitidos.Contains(novoStatus))
                throw new StatusPedidoInvalidoException(
                    $"Transição de {pedido.Status.ToApiString()} para {novoStatus.ToApiString()} não é permitida.");

            // Valida a permissão do perfil para o status alvo.
            if (requesterRole is not (Roles.Admin or Roles.Gerente))
            {
                var permitido = novoStatus switch
                {
                    StatusPedidoEnum.EmPreparo or StatusPedidoEnum.Pronto => requesterRole == Roles.Cozinha,
                    StatusPedidoEnum.Entregue => requesterRole is Roles.Atendente or Roles.Cozinha,
                    _ => false
                };

                if (!permitido)
                    throw new ForbiddenException($"Seu perfil não pode alterar o pedido para {novoStatus.ToApiString()}.");
            }

            var statusAnterior = pedido.Status;
            pedido.Status = novoStatus;
            await pedidoRepository.UpdateAsync(pedido);

            await auditoriaService.RegistrarAsync(
                acao: "ALTERAR_STATUS_PEDIDO",
                entidade: nameof(Pedido),
                entidadeId: pedido.Id,
                usuarioId: usuarioId,
                ip: ip,
                antes: new { status = statusAnterior.ToApiString() },
                depois: new { status = novoStatus.ToApiString() });

            await unitOfWork.CommitAsync();

            return new PedidoResponse
            {
                PedidoId = pedido.Id,
                Status = pedido.Status.ToApiString(),
                CanalPedido = pedido.CanalPedido.ToApiString(),
                UnidadeId = pedido.UnidadeId,
                ClienteId = pedido.ClienteId,
                Total = pedido.Total,
                CreatedAt = pedido.CreatedAt,
                Pagamento = pedido.Pagamento is null ? null : new PagamentoResponse
                {
                    Id = pedido.Pagamento.Id,
                    PedidoId = pedido.Pagamento.PedidoId,
                    Status = pedido.Pagamento.Status.ToApiString(),
                    FormaPagamento = pedido.Pagamento.FormaPagamento,
                    TransactionId = pedido.Pagamento.GatewayTransactionId,
                    CreatedAt = pedido.Pagamento.CreatedAt
                },
                Itens = pedido.Itens.Select(i => new ItemPedidoResponse
                {
                    ProdutoId = i.ProdutoId,
                    Nome = i.Produto?.Nome ?? string.Empty,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario,
                    Subtotal = i.Subtotal
                }).ToList()
            };
        }

        public async Task<PedidoResponse> CancelarPedido(long pedidoId, long usuarioId, string requesterRole, string? ip)
        {
            var pedido = await pedidoRepository.GetCompletoAsync(pedidoId);
            if (pedido is null) throw new NotFoundException("Pedido não encontrado");

            if (pedido.Status == StatusPedidoEnum.Cancelado)
                throw new StatusPedidoInvalidoException($"O pedido já está no status {StatusPedidoEnum.Cancelado.ToApiString()}.");

            if (!Transicoes.TryGetValue(pedido.Status, out var permitidos) || !permitidos.Contains(StatusPedidoEnum.Cancelado))
                throw new StatusPedidoInvalidoException(
                    $"Transição de {pedido.Status.ToApiString()} para {StatusPedidoEnum.Cancelado.ToApiString()} não é permitida.");

            await unitOfWork.BeginTransactionAsync();

            var estavaPago = pedido.Status == StatusPedidoEnum.Pago;

            if (estavaPago)
            {
                foreach (var item in pedido.Itens)
                {
                    var estoque = await estoqueRepository.GetByUnidadeProdutoAsync(pedido.UnidadeId, item.ProdutoId);
                    if (estoque is null) continue;

                    estoque.QuantidadeAtual += item.Quantidade;
                    await estoqueRepository.UpdateAsync(estoque);

                    await movimentoEstoqueRepository.AddAsync(new MovimentoEstoque
                    {
                        EstoqueId = estoque.Id,
                        TipoMovimento = TipoMovimentoEnum.Entrada,
                        Quantidade = item.Quantidade,
                        Motivo = $"Estorno do cancelamento do pedido #{pedido.Id}",
                        UsuarioId = usuarioId
                    });
                }
            }

            var statusAnterior = pedido.Status;
            pedido.Status = StatusPedidoEnum.Cancelado;
            await pedidoRepository.UpdateAsync(pedido);

            await auditoriaService.RegistrarAsync(
                acao: "CANCELAR_PEDIDO",
                entidade: nameof(Pedido),
                entidadeId: pedido.Id,
                usuarioId: usuarioId,
                ip: ip,
                antes: new { status = statusAnterior.ToApiString() },
                depois: new { status = StatusPedidoEnum.Cancelado.ToApiString() });

            await unitOfWork.CommitAsync();

            return new PedidoResponse
            {
                PedidoId = pedido.Id,
                Status = pedido.Status.ToApiString(),
                CanalPedido = pedido.CanalPedido.ToApiString(),
                UnidadeId = pedido.UnidadeId,
                ClienteId = pedido.ClienteId,
                Total = pedido.Total,
                CreatedAt = pedido.CreatedAt,
                Pagamento = pedido.Pagamento is null ? null : new PagamentoResponse
                {
                    Id = pedido.Pagamento.Id,
                    PedidoId = pedido.Pagamento.PedidoId,
                    Status = pedido.Pagamento.Status.ToApiString(),
                    FormaPagamento = pedido.Pagamento.FormaPagamento,
                    TransactionId = pedido.Pagamento.GatewayTransactionId,
                    CreatedAt = pedido.Pagamento.CreatedAt
                },
                Itens = pedido.Itens.Select(i => new ItemPedidoResponse
                {
                    ProdutoId = i.ProdutoId,
                    Nome = i.Produto?.Nome ?? string.Empty,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario,
                    Subtotal = i.Subtotal
                }).ToList()
            };
        }

        private sealed record LinhaPedido(Produto Produto, Estoque? Estoque, int Quantidade, decimal PrecoUnitario)
        {
            public decimal Subtotal => PrecoUnitario * Quantidade;
        }
    }
}
