using Core.Data;
using Domain.Contracts.Responses;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProdutoRepository(DatabaseContext context) : GenericRepository<Produto>(context), IProdutoRepository
    {
        public async Task<ResultPaginado<ProdutoResponse>> ListPaginatedAsync(int pagina = 1, int tamanhoPagina = 10)
        {
            return await _dbSet
                .AsNoTracking()
                .OrderBy(x => x.Nome)
                .Select(x => new ProdutoResponse()
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    Descricao = x.Descricao,
                    Preco = x.Preco,
                    Categoria = x.Categoria,
                    Ativo = x.Ativo,
                    Sazonal = x.Sazonal
                })
                .ToResultPaginadoAsync(pagina, tamanhoPagina);
        }
    }
}
