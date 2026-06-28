using Core.Data;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class FidelidadeContaRepository(DatabaseContext context) : GenericRepository<FidelidadeConta>(context), IFidelidadeContaRepository
    {
        public async Task<FidelidadeConta?> GetByClienteIdAsync(long clienteId)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.ClienteId == clienteId);
        }
    }
}
