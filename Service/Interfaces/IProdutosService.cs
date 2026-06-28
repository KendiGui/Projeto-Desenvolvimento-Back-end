using Domain.Contracts.Requests;
using Domain.Contracts.Responses;

namespace Service.Interfaces
{
    public interface IProdutosService
    {
        Task<ResultPaginado<ProdutoResponse>> ListaProdutos(int pagina = 1, int tamanhoPagina = 10);
        Task<ProdutoResponse> GetProduto(long produtoId);
        Task<ProdutoResponse> CadastraProduto(ProdutoRequest request);
        Task<ProdutoResponse> AtualizaProduto(long produtoId, ProdutoRequest request);
        Task RemoveProduto(long produtoId);
    }
}
