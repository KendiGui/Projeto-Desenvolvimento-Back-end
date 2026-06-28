using Domain.Contracts.Responses;

namespace Service.Interfaces
{
    public interface IAuditoriaService
    {
        /// <summary>
        /// Adiciona um registro de auditoria ao contexto. Não persiste por si
        /// só — o commit fica a cargo do serviço orquestrador (mesma transação).
        /// </summary>
        Task RegistrarAsync(string acao, string entidade, long? entidadeId, long? usuarioId, string? ip, object? antes = null, object? depois = null);

        Task<ResultPaginado<AuditoriaResponse>> ListarAsync(string? entidade, long? entidadeId, int pagina = 1, int tamanhoPagina = 10);
    }
}
