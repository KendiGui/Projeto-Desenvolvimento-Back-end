using Core.Data;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IFidelidadeHistoricoRepository : IGenericRepository<FidelidadeHistorico>
    {
        Task<IEnumerable<FidelidadeHistorico>> ListByClienteAsync(long clienteId);
    }
}
