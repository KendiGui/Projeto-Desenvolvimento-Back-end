using Core.Data;
using Domain.Contracts.Responses;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProdutoRepository(DatabaseContext context) : GenericRepository<Produto>(context), IProdutoRepository
    {
        public async Task<ResultPaginado<ProdutoResponse>> ListPaginatedAsync(int pagina = 1, int tamanhoPagina = 10)
        {
            if (pagina < 1) pagina = 1;

            if (tamanhoPagina < 1) tamanhoPagina = 10;

            var totalRegistros = await _dbSet.CountAsync();

            var itens = await _dbSet
                .AsNoTracking()
                .OrderBy(x => x.Nome)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
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
            .ToListAsync();

            return new ResultPaginado<ProdutoResponse>
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
