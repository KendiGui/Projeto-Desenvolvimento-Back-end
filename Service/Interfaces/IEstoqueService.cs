using Domain.Contracts.Requests;
using Domain.Contracts.Responses;

namespace Service.Interfaces
{
    public interface IEstoqueService
    {
        Task<ResultPaginado<EstoqueResponse>> GetEstoquePorUnidade(long unidadeId, int pagina = 1, int tamanhoPagina = 10);
        Task<MovimentoEstoqueResponse> CriarMovimento(EstoqueMovimentoRequest request, long? usuarioId, string? ip);
        Task<ResultPaginado<MovimentoEstoqueResponse>> ListaMovimentos(long? unidadeId, long? produtoId, int pagina = 1, int tamanhoPagina = 10);
    }
}
