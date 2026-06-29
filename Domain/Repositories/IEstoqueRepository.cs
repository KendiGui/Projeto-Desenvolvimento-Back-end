using Core.Data;
using Domain.Contracts.Responses;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IEstoqueRepository : IGenericRepository<Estoque>
    {
        Task<Estoque?> GetByUnidadeProdutoAsync(long unidadeId, long produtoId);
        Task<ResultPaginado<Estoque>> ListByUnidadeAsync(long unidadeId, int pagina, int tamanhoPagina);
    }
}
