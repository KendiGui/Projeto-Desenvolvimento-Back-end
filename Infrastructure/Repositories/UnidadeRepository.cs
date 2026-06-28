using Core.Data;
using Domain.Contracts.Responses;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UnidadeRepository(DatabaseContext context) : GenericRepository<Unidade>(context), IUnidadeRepository
    {
        public async Task<ResultPaginado<UnidadeResponse>> ListPaginatedAsync(int pagina = 1, int tamanhoPagina = 10)
        {
            if (pagina < 1) pagina = 1;

            if (tamanhoPagina < 1) tamanhoPagina = 10;

            var totalRegistros = await _dbSet.CountAsync();

            var itens = await _dbSet
                .AsNoTracking()
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .Select(x => new UnidadeResponse()
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    Cidade = x.Cidade,
                    Estado = x.Estado,
                    Endereco = x.Endereco,
                    Ativa = x.Ativa
                })
            .ToListAsync();

            return new ResultPaginado<UnidadeResponse>
            {
                Items = itens,
                Pagina = pagina,
                TamanhoPagina = tamanhoPagina,
                TotalItems = totalRegistros,
                TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamanhoPagina)
            };
        }
    }
}
