using Core.Data;
using Domain.Contracts.Responses;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UnidadeRepository(DatabaseContext context) : GenericRepository<Unidade>(context), IUnidadeRepository
    {
        public async Task<ResultPaginado<UnidadeResponse>> ListPaginatedAsync(int pagina = 1, int tamanhoPagina = 10)
        {
            return await _dbSet
                .AsNoTracking()
                .OrderBy(x => x.Nome)
                .Select(x => new UnidadeResponse()
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    Cidade = x.Cidade,
                    Estado = x.Estado,
                    Endereco = x.Endereco,
                    Ativa = x.Ativa
                })
                .ToResultPaginadoAsync(pagina, tamanhoPagina);
        }
    }
}
