using Core.Data;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IEstoqueRepository : IGenericRepository<Estoque>
    {
        Task<Estoque?> GetByUnidadeProdutoAsync(long unidadeId, long produtoId);
        Task<IEnumerable<Estoque>> ListByUnidadeAsync(long unidadeId);
    }
}
