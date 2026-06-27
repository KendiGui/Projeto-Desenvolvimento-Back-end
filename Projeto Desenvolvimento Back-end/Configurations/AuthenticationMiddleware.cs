using Microsoft.AspNetCore.Authorization;

namespace Projeto_Desenvolvimento_Back_end.Configurations
{
    public class AuthenticationMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        public async Task InvokeAsync(HttpContext context)
        {

            var endpoint = context.GetEndpoint();

            var request = context.Request;

            if (endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>() != null)
            {
                await next(context);
                return;
            }

            if (context.Request.Method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
            {
                await next(context);
                return;
            }

            if (context.Request.Path.StartsWithSegments("/swagger") ||
                context.Request.Path.StartsWithSegments("/api-docs") ||
                context.Request.Path.StartsWithSegments("/hangfire"))
            {
                await next(context);
                return;
            }

            var headers = context.Request.Headers;
            var authToken = headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(authToken))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            request.EnableBuffering();
            await next(context);
        }
    }
}
