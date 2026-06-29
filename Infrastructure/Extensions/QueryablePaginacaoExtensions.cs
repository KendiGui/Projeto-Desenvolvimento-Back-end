using Domain.Contracts.Responses;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Extensions
{
    public static class QueryablePaginacaoExtensions
    {
        public static async Task<ResultPaginado<T>> ToResultPaginadoAsync<T>(this IQueryable<T> query, int pagina = 1, int tamanhoPagina = 10)
        {
            if (pagina < 1) pagina = 1;
            if (tamanhoPagina < 1) tamanhoPagina = 10;

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            return new ResultPaginado<T>
            {
                Items = items,
                Pagina = pagina,
                TamanhoPagina = tamanhoPagina,
                TotalItems = totalItems,
                TotalPaginas = (int)Math.Ceiling(totalItems / (double)tamanhoPagina)
            };
        }
    }
}
