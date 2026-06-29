using Domain.Contracts.Requests;
using Domain.Contracts.Responses;

namespace Service.Interfaces
{
    public interface IFidelidadeService
    {
        Task<FidelidadeSaldoResponse> GetSaldo(long clienteId);
        Task<ResultPaginado<FidelidadeHistoricoResponse>> GetHistorico(long clienteId, int pagina = 1, int tamanhoPagina = 10);
        Task<FidelidadeSaldoResponse> Resgatar(long clienteId, ResgatarPontosRequest request);
        Task<FidelidadeSaldoResponse> AtualizarConsentimento(long clienteId, ConsentimentoRequest request);
        Task AcumularPontosAsync(long clienteId, long pedidoId, decimal totalPedido);
    }
}
