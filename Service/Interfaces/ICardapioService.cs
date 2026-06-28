using Domain.Contracts.Requests;
using Domain.Contracts.Responses;

namespace Service.Interfaces
{
    public interface ICardapioService
    {
        Task<IEnumerable<CardapioItemResponse>> GetCardapio(long unidadeId);
        Task<CardapioItemResponse> AdicionaProduto(long unidadeId, CardapioProdutoRequest request);
        Task<CardapioItemResponse> AtualizaProduto(long unidadeId, long produtoId, CardapioProdutoRequest request);
    }
}
