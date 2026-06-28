using Core.Data;
using Domain.Contracts.Responses;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UnidadeProdutoRepository(DatabaseContext context) : GenericRepository<UnidadeProduto>(context), IUnidadeProdutoRepository
    {
        public async Task<IEnumerable<CardapioItemResponse>> GetCardapioAsync(long unidadeId, bool apenasDisponiveis = false)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(up => up.UnidadeId == unidadeId);

            if (apenasDisponiveis)
                query = query.Where(up => up.Disponivel && up.Produto.Ativo);

            return await query
                .OrderBy(up => up.Produto.Nome)
                .Select(up => new CardapioItemResponse
                {
                    ProdutoId = up.ProdutoId,
                    Nome = up.Produto.Nome,
                    Descricao = up.Produto.Descricao,
                    Categoria = up.Produto.Categoria,
                    Preco = up.PrecoCustomizado ?? up.Produto.Preco,
                    Disponivel = up.Disponivel,
                    Sazonal = up.Produto.Sazonal
                })
            .ToListAsync();
        }

        public async Task<UnidadeProduto?> GetByUnidadeProdutoAsync(long unidadeId, long produtoId)
        {
            return await _dbSet.FirstOrDefaultAsync(up => up.UnidadeId == unidadeId && up.ProdutoId == produtoId);
        }
    }
}
