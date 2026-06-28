using Domain.Contracts.Requests;
using Domain.Contracts.Responses;
using Domain.Entities;

namespace Service.Interfaces
{
    public interface IUnidadesService
    {
        Task<ResultPaginado<UnidadeResponse>> ListaUnidades(int pagina = 1, int tamanhoPagina = 10);
        Task<UnidadeResponse> GetUnidade(long unidadeId);
        Task<UnidadeResponse> CadastraUnidade(UnidadeRequest request);
        Task<UnidadeResponse> AtualizaUnidade(long unidadeId, UnidadeRequest request);
    }
}
