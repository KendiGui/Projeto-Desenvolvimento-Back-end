using Core.Data;
using Domain.Contracts.Responses;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IMovimentoEstoqueRepository : IGenericRepository<MovimentoEstoque>
    {
        Task<ResultPaginado<MovimentoEstoque>> ListFiltradoAsync(long? unidadeId, long? produtoId, int pagina, int tamanhoPagina);
    }
}
