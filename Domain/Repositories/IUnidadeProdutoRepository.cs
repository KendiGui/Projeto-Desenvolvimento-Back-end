using Core.Data;
using Domain.Contracts.Responses;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUnidadeProdutoRepository : IGenericRepository<UnidadeProduto>
    {
        Task<ResultPaginado<CardapioItemResponse>> GetCardapioAsync(long unidadeId, int pagina, int tamanhoPagina, bool apenasDisponiveis = false);
        Task<UnidadeProduto?> GetByUnidadeProdutoAsync(long unidadeId, long produtoId);
    }
}
