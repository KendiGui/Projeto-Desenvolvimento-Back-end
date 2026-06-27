using Domain.Entities;

namespace Service.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(Usuario usuario);
        DateTime GetTokenExpiryTime();
    }
}
