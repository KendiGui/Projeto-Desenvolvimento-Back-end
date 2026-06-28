using Core.Data;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AuditoriaRepository(DatabaseContext context) : GenericRepository<Auditoria>(context), IAuditoriaRepository
    {
        public async Task<(IEnumerable<Auditoria> Items, int Total)> ListFiltradoAsync(
            string? entidade,
            long? entidadeId,
            int pagina,
            int tamanhoPagina)
        {
            if (pagina < 1) pagina = 1;
            if (tamanhoPagina < 1) tamanhoPagina = 10;

            var query = _dbSet.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(entidade))
                query = query.Where(a => a.Entidade == entidade);

            if (entidadeId.HasValue)
                query = query.Where(a => a.EntidadeId == entidadeId.Value);

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            return (items, total);
        }
    }
}
