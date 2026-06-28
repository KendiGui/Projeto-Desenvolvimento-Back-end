using Domain.Contracts.Requests;
using Domain.Contracts.Responses;

namespace Service.Interfaces
{
    public interface IEstoqueService
    {
        Task<IEnumerable<EstoqueResponse>> GetEstoquePorUnidade(long unidadeId);
        Task<MovimentoEstoqueResponse> CriarMovimento(EstoqueMovimentoRequest request, long? usuarioId, string? ip);
        Task<IEnumerable<MovimentoEstoqueResponse>> ListaMovimentos(long? unidadeId, long? produtoId);
    }
}
