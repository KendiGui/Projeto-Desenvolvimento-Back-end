using Core.Data;
using Domain.Contracts.Responses;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUnidadeProdutoRepository : IGenericRepository<UnidadeProduto>
    {
        Task<IEnumerable<CardapioItemResponse>> GetCardapioAsync(long unidadeId, bool apenasDisponiveis = false);
        Task<UnidadeProduto?> GetByUnidadeProdutoAsync(long unidadeId, long produtoId);
    }
}
