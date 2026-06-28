using Core.Data;
using Domain.Contracts.Responses;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUnidadeRepository : IGenericRepository<Unidade>
    {
        Task<ResultPaginado<UnidadeResponse>> ListPaginatedAsync(int pagina = 1, int tamanhoPagina = 10);
    }
}
