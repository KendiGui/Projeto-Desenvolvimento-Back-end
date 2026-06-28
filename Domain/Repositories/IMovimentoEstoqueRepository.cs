using Core.Data;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IMovimentoEstoqueRepository : IGenericRepository<MovimentoEstoque>
    {
        Task<IEnumerable<MovimentoEstoque>> ListFiltradoAsync(long? unidadeId, long? produtoId);
    }
}
