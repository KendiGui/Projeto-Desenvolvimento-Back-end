using Core.Data;
using Domain.Contracts.Requests;
using Domain.Contracts.Responses;
using Domain.Entities;
using Domain.Repositories;
using Service.Interfaces;

namespace Service.Services
{
    public class AuthService(IUsuarioRepository usuarioRepository, IJwtTokenService tokenService, IUnitOfWork unitOfWork) : IAuthService
    {
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha) || string.IsNullOrWhiteSpace(request.Nome))
            {
                throw new ArgumentException("Nome, Email e Senha são obrigatórios.");
            }

            var emailExists = await usuarioRepository.EmailExistsAsync(request.Email);
            if (emailExists)
            {
                throw new InvalidOperationException("Email já registrado.");
            }

            var senhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha);

            var usuario = new Usuario
            {
                Nome = request.Nome,
                Email = request.Email,
                SenhaHash = senhaHash,
                Role = request.Role,
                Ativo = true
            };

            await usuarioRepository.AddAsync(usuario);
            await unitOfWork.CommitAsync();

            var token = tokenService.GenerateToken(usuario);

            return new AuthResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Role = usuario.Role,
                Token = token,
                ExpiresAt = tokenService.GetTokenExpiryTime()
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha))
            {
                throw new ArgumentException("Email e Senha são obrigatórios.");
            }

            var usuario = await usuarioRepository.GetByEmailAsync(request.Email);
            if (usuario == null || !usuario.Ativo)
            {
                throw new UnauthorizedAccessException("Email ou senha inválidos.");
            }

            var senhaValida = BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash);
            if (!senhaValida)
            {
                throw new UnauthorizedAccessException("Email ou senha inválidos.");
            }

            var token = tokenService.GenerateToken(usuario);

            return new AuthResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Role = usuario.Role,
                Token = token,
                ExpiresAt = tokenService.GetTokenExpiryTime()
            };
        }

        public async Task<UsuarioResponse> GetMeAsync(long usuarioId)
        {
            var usuario = await usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null)
            {
                throw new InvalidOperationException("Usuário não encontrado.");
            }

            return new UsuarioResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Role = usuario.Role,
                Ativo = usuario.Ativo
            };
        }
    }
}
