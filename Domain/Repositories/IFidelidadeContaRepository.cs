using Core.Data;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IFidelidadeContaRepository : IGenericRepository<FidelidadeConta>
    {
        Task<FidelidadeConta?> GetByClienteIdAsync(long clienteId);
    }
}
