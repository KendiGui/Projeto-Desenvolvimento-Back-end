using Core.Data;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class FidelidadeHistoricoRepository(DatabaseContext context) : GenericRepository<FidelidadeHistorico>(context), IFidelidadeHistoricoRepository
    {
        public async Task<IEnumerable<FidelidadeHistorico>> ListByClienteAsync(long clienteId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(h => h.ClienteId == clienteId)
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();
        }
    }
}
