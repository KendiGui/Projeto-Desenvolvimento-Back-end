using System.Text.Json;
using Domain.Contracts;
using Domain.Contracts.Exceptions;

namespace Projeto_Desenvolvimento_Back_end.Middlewares
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await EscreverRespostaAsync(context, ex);
            }
        }

        private async Task EscreverRespostaAsync(HttpContext context, Exception ex)
        {
            var (status, codigo, detalhes) = ex switch
            {
                BusinessException be => (be.StatusCode, be.Codigo, be.Detalhes),
                ArgumentException => (StatusCodes.Status400BadRequest, "VALIDATION_ERROR", (IEnumerable<ErroDetalhe>?)null),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "UNAUTHORIZED", null),
                _ => (StatusCodes.Status500InternalServerError, "ERRO_INTERNO", null)
            };

            if (status >= 500)
                logger.LogError(ex, "Erro não tratado em {Path}", context.Request.Path);
            else
                logger.LogWarning("{Codigo} em {Path}: {Mensagem}", codigo, context.Request.Path, ex.Message);

            context.Response.StatusCode = status;

            if (status == StatusCodes.Status204NoContent) return;

            context.Response.ContentType = "application/json";

            var corpo = new ErroResponse(codigo, ex.Message)
            {
                Detalhes = detalhes,
                Path = context.Request.Path,
                RequestId = context.TraceIdentifier
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(corpo, JsonOptions));
        }
    }
}
