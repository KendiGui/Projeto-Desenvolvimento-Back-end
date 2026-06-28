using Core.Data;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MovimentoEstoqueRepository(DatabaseContext context) : GenericRepository<MovimentoEstoque>(context), IMovimentoEstoqueRepository
    {
        public async Task<IEnumerable<MovimentoEstoque>> ListFiltradoAsync(long? unidadeId, long? produtoId)
        {
            var query = _dbSet
                .AsNoTracking()
                .Include(m => m.Estoque)
            .AsQueryable();

            if (unidadeId.HasValue)
                query = query.Where(m => m.Estoque.UnidadeId == unidadeId.Value);

            if (produtoId.HasValue)
                query = query.Where(m => m.Estoque.ProdutoId == produtoId.Value);

            return await query
                .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
        }
    }
}
