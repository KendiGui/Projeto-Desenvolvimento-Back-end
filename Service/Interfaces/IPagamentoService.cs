using Domain.Contracts.Responses;

namespace Service.Interfaces
{
    public interface IPagamentoService
    {
        Task<PagamentoResponse> GetPagamentoDoPedido(long pedidoId, long requesterId, string requesterRole);
    }
}
