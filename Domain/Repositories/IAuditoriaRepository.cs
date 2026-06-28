using Core.Data;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IAuditoriaRepository : IGenericRepository<Auditoria>
    {
        Task<(IEnumerable<Auditoria> Items, int Total)> ListFiltradoAsync(
            string? entidade,
            long? entidadeId,
            int pagina,
            int tamanhoPagina);
    }
}
