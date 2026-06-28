using Core.Data;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EstoqueRepository(DatabaseContext context) : GenericRepository<Estoque>(context), IEstoqueRepository
    {
        public async Task<Estoque?> GetByUnidadeProdutoAsync(long unidadeId, long produtoId)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.UnidadeId == unidadeId && e.ProdutoId == produtoId);
        }

        public async Task<IEnumerable<Estoque>> ListByUnidadeAsync(long unidadeId)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(e => e.Produto)
                .Where(e => e.UnidadeId == unidadeId)
                .OrderBy(e => e.Produto.Nome)
            .ToListAsync();
        }
    }
}
