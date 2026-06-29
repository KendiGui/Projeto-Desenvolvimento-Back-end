using Core.Data;
using Domain.Contracts.Responses;
using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories
{
    public interface IPedidoRepository : IGenericRepository<Pedido>
    {
        Task<Pedido?> GetCompletoAsync(long id);

        Task<ResultPaginado<Pedido>> ListFiltradoAsync(
            long? clienteId,
            CanalPedidoEnum? canalPedido,
            StatusPedidoEnum? status,
            int pagina,
            int tamanhoPagina);
    }
}
