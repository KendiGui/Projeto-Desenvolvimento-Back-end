using System.Security.Claims;

namespace Projeto_Desenvolvimento_Back_end.Configurations
{
    public static class ClaimsExtensions
    {
        public static long GetUserId(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(claim, out var id))
                throw new UnauthorizedAccessException("Token inválido.");
            return id;
        }

        public static string GetUserRole(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }

        public static string? GetIp(this HttpContext context)
        {
            return context.Connection.RemoteIpAddress?.ToString();
        }
    }
}
