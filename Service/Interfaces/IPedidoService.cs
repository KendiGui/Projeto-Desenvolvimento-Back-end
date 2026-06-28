using Domain.Contracts.Requests;
using Domain.Contracts.Responses;

namespace Service.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoResponse> CriarPedido(CriarPedidoRequest request, long clienteId, string? ip);
        Task<ResultPaginado<PedidoResponse>> ListaPedidos(string? canalPedido, string? status, long requesterId, string requesterRole, int pagina = 1, int tamanhoPagina = 10);
        Task<PedidoResponse> GetPedido(long pedidoId, long requesterId, string requesterRole);
        Task<PedidoResponse> AtualizarStatus(long pedidoId, AtualizarStatusPedidoRequest request, long usuarioId, string requesterRole, string? ip);
        Task<PedidoResponse> CancelarPedido(long pedidoId, long usuarioId, string requesterRole, string? ip);
    }
}
