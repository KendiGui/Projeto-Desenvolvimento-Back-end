using Core.Data;
using Domain.Contracts.Responses;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AuditoriaRepository(DatabaseContext context) : GenericRepository<Auditoria>(context), IAuditoriaRepository
    {
        public async Task<ResultPaginado<Auditoria>> ListFiltradoAsync(
            string? entidade,
            long? entidadeId,
            int pagina,
            int tamanhoPagina)
        {
            var query = _dbSet.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(entidade))
                query = query.Where(a => a.Entidade == entidade);

            if (entidadeId.HasValue)
                query = query.Where(a => a.EntidadeId == entidadeId.Value);

            return await query
                .OrderByDescending(a => a.CreatedAt)
                .ToResultPaginadoAsync(pagina, tamanhoPagina);
        }
    }
}
