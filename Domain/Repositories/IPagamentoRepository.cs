using Core.Data;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IPagamentoRepository : IGenericRepository<Pagamento>
    {
        Task<Pagamento?> GetByPedidoIdAsync(long pedidoId);
    }
}
