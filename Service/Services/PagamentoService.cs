using Domain.Contracts.Exceptions;
using Domain.Contracts.Responses;
using Domain.Helpers;
using Domain.Repositories;
using Service.Interfaces;

namespace Service.Services
{
    public class PagamentoService(IPagamentoRepository pagamentoRepository, IPedidoRepository pedidoRepository) : IPagamentoService
    {
        public async Task<PagamentoResponse> GetPagamentoDoPedido(long pedidoId, long requesterId, string requesterRole)
        {
            var pedido = await pedidoRepository.GetByIdAsync(pedidoId);
            if (pedido is null) throw new NotFoundException("Pedido não encontrado");

            if (requesterRole == Roles.Cliente && pedido.ClienteId != requesterId)
                throw new ForbiddenException("Você não tem acesso a este pedido.");

            var pagamento = await pagamentoRepository.GetByPedidoIdAsync(pedidoId);
            if (pagamento is null) throw new NotFoundException("Pagamento não encontrado para o pedido.");

            return new PagamentoResponse
            {
                Id = pagamento.Id,
                PedidoId = pagamento.PedidoId,
                Status = pagamento.Status.ToApiString(),
                FormaPagamento = pagamento.FormaPagamento,
                TransactionId = pagamento.GatewayTransactionId,
                CreatedAt = pagamento.CreatedAt
            };
        }
    }
}
