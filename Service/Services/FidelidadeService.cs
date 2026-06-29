using Core.Data;
using Domain.Contracts.Exceptions;
using Domain.Contracts.Requests;
using Domain.Contracts.Responses;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Service.Interfaces;

namespace Service.Services
{
    public class FidelidadeService(IFidelidadeContaRepository contaRepository, IFidelidadeHistoricoRepository historicoRepository, IUnitOfWork unitOfWork) : IFidelidadeService
    {
        public async Task<FidelidadeSaldoResponse> GetSaldo(long clienteId)
        {
            var conta = await contaRepository.GetByClienteIdAsync(clienteId);
            if (conta is null)
            {
                conta = new FidelidadeConta
                {
                    ClienteId = clienteId,
                    Pontos = 0,
                    ConsentimentoMarketing = false
                };
                await contaRepository.AddAsync(conta);
            }

            await unitOfWork.CommitAsync();

            return new FidelidadeSaldoResponse
            {
                ClienteId = conta.ClienteId,
                Pontos = conta.Pontos,
                ConsentimentoMarketing = conta.ConsentimentoMarketing,
                ConsentimentoData = conta.ConsentimentoData
            };
        }

        public async Task<ResultPaginado<FidelidadeHistoricoResponse>> GetHistorico(long clienteId, int pagina = 1, int tamanhoPagina = 10)
        {
            var historicos = await historicoRepository.ListByClienteAsync(clienteId, pagina, tamanhoPagina);

            if (!historicos.Items.Any()) throw new EmptyListException("Nenhum histórico de fidelidade encontrado");

            return historicos.Map(h => new FidelidadeHistoricoResponse
            {
                Id = h.Id,
                Tipo = h.Tipo.ToString().ToUpperInvariant(),
                Pontos = h.Pontos,
                Descricao = h.Descricao,
                PedidoId = h.PedidoId,
                CreatedAt = h.CreatedAt
            });
        }

        public async Task<FidelidadeSaldoResponse> Resgatar(long clienteId, ResgatarPontosRequest request)
        {
            if (request.Pontos <= 0)
                throw new ValidationException("A quantidade de pontos a resgatar deve ser maior que zero.",
                    [new("pontos", "Deve ser maior que zero")]);

            var conta = await contaRepository.GetByClienteIdAsync(clienteId);
            if (conta is null)
            {
                conta = new FidelidadeConta
                {
                    ClienteId = clienteId,
                    Pontos = 0,
                    ConsentimentoMarketing = false
                };
                await contaRepository.AddAsync(conta);
            }

            if (request.Pontos > conta.Pontos)
                throw new ValidationException("Pontos insuficientes para resgate.",
                    [new("pontos", $"Disponível: {conta.Pontos}")]);

            conta.Pontos -= request.Pontos;
            await contaRepository.UpdateAsync(conta);

            await historicoRepository.AddAsync(new FidelidadeHistorico
            {
                ClienteId = clienteId,
                Tipo = TipoFidelidadeEnum.Debito,
                Pontos = request.Pontos,
                Descricao = request.Descricao
            });

            await unitOfWork.CommitAsync();

            return new FidelidadeSaldoResponse
            {
                ClienteId = conta.ClienteId,
                Pontos = conta.Pontos,
                ConsentimentoMarketing = conta.ConsentimentoMarketing,
                ConsentimentoData = conta.ConsentimentoData
            };
        }

        public async Task<FidelidadeSaldoResponse> AtualizarConsentimento(long clienteId, ConsentimentoRequest request)
        {
            var conta = await contaRepository.GetByClienteIdAsync(clienteId);
            if (conta is null)
            {
                conta = new FidelidadeConta
                {
                    ClienteId = clienteId,
                    Pontos = 0,
                    ConsentimentoMarketing = false
                };
                await contaRepository.AddAsync(conta);
            }

            conta.ConsentimentoMarketing = request.ConsentimentoMarketing;
            conta.ConsentimentoData = DateTime.UtcNow;
            await contaRepository.UpdateAsync(conta);

            await unitOfWork.CommitAsync();

            return new FidelidadeSaldoResponse
            {
                ClienteId = conta.ClienteId,
                Pontos = conta.Pontos,
                ConsentimentoMarketing = conta.ConsentimentoMarketing,
                ConsentimentoData = conta.ConsentimentoData
            };
        }

        public async Task AcumularPontosAsync(long clienteId, long pedidoId, decimal totalPedido)
        {
            var pontos = (int)Math.Floor(totalPedido);
            if (pontos <= 0) return;

            var conta = await contaRepository.GetByClienteIdAsync(clienteId);
            if (conta is null)
            {
                conta = new FidelidadeConta
                {
                    ClienteId = clienteId,
                    Pontos = pontos,
                    ConsentimentoMarketing = false
                };
                await contaRepository.AddAsync(conta);
            } else
            {
                conta.Pontos += pontos;
                await contaRepository.UpdateAsync(conta);
            }

            await historicoRepository.AddAsync(new FidelidadeHistorico
            {
                ClienteId = clienteId,
                PedidoId = pedidoId,
                Tipo = TipoFidelidadeEnum.Credito,
                Pontos = pontos,
                Descricao = $"Pontos do pedido #{pedidoId}"
            });
        }
    }
}
