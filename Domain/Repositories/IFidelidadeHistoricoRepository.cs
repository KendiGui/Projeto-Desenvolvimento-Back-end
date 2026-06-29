using Core.Data;
using Domain.Contracts.Responses;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IFidelidadeHistoricoRepository : IGenericRepository<FidelidadeHistorico>
    {
        Task<ResultPaginado<FidelidadeHistorico>> ListByClienteAsync(long clienteId, int pagina, int tamanhoPagina);
    }
}
