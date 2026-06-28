using System.Text.Json;
using Domain.Contracts.Responses;
using Domain.Entities;
using Domain.Repositories;
using Service.Interfaces;

namespace Service.Services
{
    public class AuditoriaService(IAuditoriaRepository auditoriaRepository) : IAuditoriaService
    {
        public async Task RegistrarAsync(string acao, string entidade, long? entidadeId, long? usuarioId, string? ip, object? antes = null, object? depois = null)
        {
            var auditoria = new Auditoria
            {
                Acao = acao,
                Entidade = entidade,
                EntidadeId = entidadeId,
                UsuarioId = usuarioId,
                Ip = ip,
                DadosAntes = antes is null ? null : JsonSerializer.Serialize(antes),
                DadosDepois = depois is null ? null : JsonSerializer.Serialize(depois)
            };

            await auditoriaRepository.AddAsync(auditoria);
        }

        public async Task<ResultPaginado<AuditoriaResponse>> ListarAsync(string? entidade, long? entidadeId, int pagina = 1, int tamanhoPagina = 10)
        {
            var (items, total) = await auditoriaRepository.ListFiltradoAsync(entidade, entidadeId, pagina, tamanhoPagina);

            return new ResultPaginado<AuditoriaResponse>
            {
                Items = items.Select(a => new AuditoriaResponse
                {
                    Id = a.Id,
                    UsuarioId = a.UsuarioId,
                    Acao = a.Acao,
                    Entidade = a.Entidade,
                    EntidadeId = a.EntidadeId,
                    DadosAntes = a.DadosAntes,
                    DadosDepois = a.DadosDepois,
                    Ip = a.Ip,
                    CreatedAt = a.CreatedAt
                }),
                Pagina = pagina < 1 ? 1 : pagina,
                TamanhoPagina = tamanhoPagina < 1 ? 10 : tamanhoPagina,
                TotalItems = total,
                TotalPaginas = (int)Math.Ceiling(total / (double)(tamanhoPagina < 1 ? 10 : tamanhoPagina))
            };
        }
    }
}
