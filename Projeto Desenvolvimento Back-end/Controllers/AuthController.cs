using Domain.Contracts;
using Domain.Contracts.Requests;
using Domain.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Projeto_Desenvolvimento_Back_end.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
    {

        [HttpPost("register")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Realiza o cadastro de usuários")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            logger.LogInformation("Tentativa de registro para email: {Email}", request.Email);
            var result = await authService.RegisterAsync(request);
            return CreatedAtAction(nameof(GetMe), result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Realiza o login de um usuário e retorna o token de autorização")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            logger.LogInformation("Tentativa de login para email: {Email}", request.Email);
            var result = await authService.LoginAsync(request);
            return Ok(result);
        }

        [HttpGet("me")]
        [Authorize]
        [SwaggerOperation(Summary = "Retorna os dados do usuário autenticado.")]
        [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetMe()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdClaim, out var userId))
            {
                logger.LogWarning("Falha ao extrair ID do usuário do token");
                return Unauthorized(new ErroResponse("NAO_AUTORIZADO", "Token inválido."));
            }

            var result = await authService.GetMeAsync(userId);
            return Ok(result);
        }
    }
}
