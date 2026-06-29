using Domain.Contracts.Requests;
using Domain.Contracts.Responses;

namespace Service.Interfaces
{
    public interface ICardapioService
    {
        Task<ResultPaginado<CardapioItemResponse>> GetCardapio(long unidadeId, int pagina = 1, int tamanhoPagina = 10);
        Task<CardapioItemResponse> AdicionaProduto(long unidadeId, CardapioProdutoRequest request);
        Task<CardapioItemResponse> AtualizaProduto(long unidadeId, long produtoId, CardapioProdutoRequest request);
    }
}
