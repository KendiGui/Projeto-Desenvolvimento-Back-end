using Core.Data;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PagamentoRepository(DatabaseContext context) : GenericRepository<Pagamento>(context), IPagamentoRepository
    {
        public async Task<Pagamento?> GetByPedidoIdAsync(long pedidoId)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(p => p.PedidoId == pedidoId);
        }
    }
}
