using Core.Data;
using Domain.Contracts.Responses;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IProdutoRepository : IGenericRepository<Produto>
    {
        Task<ResultPaginado<ProdutoResponse>> ListPaginatedAsync(int pagina = 1, int tamanhoPagina = 10);
    }
}
