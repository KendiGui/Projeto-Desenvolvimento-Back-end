using Core.Data;
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
    public class EstoqueService(IEstoqueRepository estoqueRepository, IMovimentoEstoqueRepository movimentoEstoqueRepository, IUnidadeRepository unidadeRepository, IProdutoRepository produtoRepository, IAuditoriaService auditoriaService, IUnitOfWork unitOfWork) : IEstoqueService
    {
        public async Task<ResultPaginado<EstoqueResponse>> GetEstoquePorUnidade(long unidadeId, int pagina = 1, int tamanhoPagina = 10)
        {
            var unidade = await unidadeRepository.GetByIdAsync(unidadeId);
            if (unidade is null) throw new NotFoundException("Unidade não encontrada");

            var estoques = await estoqueRepository.ListByUnidadeAsync(unidadeId, pagina, tamanhoPagina);

            return estoques.Map(e => new EstoqueResponse
            {
                EstoqueId = e.Id,
                UnidadeId = e.UnidadeId,
                ProdutoId = e.ProdutoId,
                ProdutoNome = e.Produto?.Nome ?? string.Empty,
                QuantidadeAtual = e.QuantidadeAtual,
                QuantidadeMinima = e.QuantidadeMinima
            });
        }

        public async Task<MovimentoEstoqueResponse> CriarMovimento(EstoqueMovimentoRequest request, long? usuarioId, string? ip)
        {
            if (!EnumExtensions.TryParseApi<TipoMovimentoEnum>(request.TipoMovimento, out var tipo))
                throw new ValidationException("Tipo de movimento inválido. Use ENTRADA, SAIDA ou AJUSTE.",
                    [new("tipoMovimento", "Valor inválido")]);

            if (tipo != TipoMovimentoEnum.Ajuste && request.Quantidade <= 0)
                throw new ValidationException("Quantidade deve ser maior que zero.",
                    [new("quantidade", "Deve ser maior que zero")]);

            if (tipo == TipoMovimentoEnum.Ajuste && request.Quantidade < 0)
                throw new ValidationException("Quantidade de ajuste não pode ser negativa.",
                    [new("quantidade", "Deve ser maior ou igual a zero")]);

            var unidade = await unidadeRepository.GetByIdAsync(request.UnidadeId);
            if (unidade is null) throw new NotFoundException("Unidade não encontrada");

            var produto = await produtoRepository.GetByIdAsync(request.ProdutoId);
            if (produto is null) throw new NotFoundException("Produto não encontrado");

            var estoque = await estoqueRepository.GetByUnidadeProdutoAsync(request.UnidadeId, request.ProdutoId);

            if (estoque is null)
            {
                if (tipo == TipoMovimentoEnum.Saida)
                    throw new EstoqueInsuficienteException("Não há estoque para o produto informado nesta unidade.");

                estoque = new Estoque
                {
                    UnidadeId = request.UnidadeId,
                    ProdutoId = request.ProdutoId,
                    QuantidadeAtual = 0,
                    QuantidadeMinima = 0
                };
                await estoqueRepository.AddAsync(estoque);
            }

            var quantidadeAntes = estoque.QuantidadeAtual;

            switch (tipo)
            {
                case TipoMovimentoEnum.Entrada:
                    estoque.QuantidadeAtual += request.Quantidade;
                    break;
                case TipoMovimentoEnum.Saida:
                    if (request.Quantidade > estoque.QuantidadeAtual)
                        throw new EstoqueInsuficienteException(
                            "Quantidade indisponível em estoque.",
                            [new("quantidade", $"Disponível: {estoque.QuantidadeAtual}")]);
                    estoque.QuantidadeAtual -= request.Quantidade;
                    break;
                case TipoMovimentoEnum.Ajuste:
                    estoque.QuantidadeAtual = request.Quantidade;
                    break;
            }

            await estoqueRepository.UpdateAsync(estoque);

            var movimento = new MovimentoEstoque
            {
                Estoque = estoque,
                TipoMovimento = tipo,
                Quantidade = request.Quantidade,
                Motivo = string.IsNullOrWhiteSpace(request.Motivo) ? tipo.ToApiString() : request.Motivo,
                UsuarioId = usuarioId
            };
            await movimentoEstoqueRepository.AddAsync(movimento);

            await auditoriaService.RegistrarAsync(
                acao: "AJUSTAR_ESTOQUE",
                entidade: nameof(Estoque),
                entidadeId: estoque.Id,
                usuarioId: usuarioId,
                ip: ip,
                antes: new { quantidade = quantidadeAntes },
                depois: new { quantidade = estoque.QuantidadeAtual, tipo = tipo.ToApiString() });

            await unitOfWork.CommitAsync();

            return new MovimentoEstoqueResponse
            {
                Id = movimento.Id,
                EstoqueId = estoque.Id,
                UnidadeId = estoque.UnidadeId,
                ProdutoId = estoque.ProdutoId,
                TipoMovimento = tipo.ToApiString(),
                Quantidade = movimento.Quantidade,
                Motivo = movimento.Motivo,
                UsuarioId = movimento.UsuarioId,
                CreatedAt = movimento.CreatedAt
            };
        }

        public async Task<ResultPaginado<MovimentoEstoqueResponse>> ListaMovimentos(long? unidadeId, long? produtoId, int pagina = 1, int tamanhoPagina = 10)
        {
            var movimentos = await movimentoEstoqueRepository.ListFiltradoAsync(unidadeId, produtoId, pagina, tamanhoPagina);

            return movimentos.Map(m => new MovimentoEstoqueResponse
            {
                Id = m.Id,
                EstoqueId = m.EstoqueId,
                UnidadeId = m.Estoque?.UnidadeId ?? 0,
                ProdutoId = m.Estoque?.ProdutoId ?? 0,
                TipoMovimento = m.TipoMovimento.ToApiString(),
                Quantidade = m.Quantidade,
                Motivo = m.Motivo,
                UsuarioId = m.UsuarioId,
                CreatedAt = m.CreatedAt
            });
        }
    }
}
