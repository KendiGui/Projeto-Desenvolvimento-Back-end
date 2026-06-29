using Core.Data;
using Domain.Contracts.Responses;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IAuditoriaRepository : IGenericRepository<Auditoria>
    {
        Task<ResultPaginado<Auditoria>> ListFiltradoAsync(
            string? entidade,
            long? entidadeId,
            int pagina,
            int tamanhoPagina);
    }
}
