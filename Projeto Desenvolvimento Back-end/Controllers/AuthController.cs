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
            try
            {
                logger.LogInformation("Tentativa de registro para email: {Email}", request.Email);
                var result = await authService.RegisterAsync(request);
                return CreatedAtAction(nameof(GetMe), result);
            }
            catch (InvalidOperationException ex)
            {
                logger.LogWarning("Email duplicado: {Message}", ex.Message);
                return Conflict(new ErroResponse("EMAIL_DUPLICADO", ex.Message));
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning("Dados inválidos: {Message}", ex.Message);
                return BadRequest(new ErroResponse("ERRO_VALIDACAO", ex.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao registrar usuário");
                return StatusCode(500, new ErroResponse("ERRO_INTERNO", ex.Message));
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Realiza o login de um usuário e retorna o token de autorização")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                logger.LogInformation("Tentativa de login para email: {Email}", request.Email);
                var result = await authService.LoginAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning("Dados inválidos: {Message}", ex.Message);
                return BadRequest(new ErroResponse("ERRO_VALIDACAO", ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.LogWarning("Falha na autenticação: {Message}", ex.Message);
                return Unauthorized(new ErroResponse("NAO_AUTORIZADO", ex.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao fazer login");
                return StatusCode(500, new ErroResponse("ERRO_INTERNO", ex.Message));
            }
        }

        /// <summary>
        /// Retorna os dados do usuário autenticado
        /// </summary>
        /// <returns>Dados do usuário</returns>
        /// <response code="200">Dados do usuário retornados com sucesso</response>
        /// <response code="401">Usuário não autenticado</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpGet("me")]
        [Authorize]
        [SwaggerOperation(Summary = "Retorna os dados do usuário autenticado.")]
        [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetMe()
        {
            try
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
            catch (InvalidOperationException ex)
            {
                logger.LogWarning("Usuário não encontrado: {Message}", ex.Message);
                return NotFound(new ErroResponse("USUARIO_NAO_ENCONTRADO", ex.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao obter dados do usuário");
                return StatusCode(500, new ErroResponse("ERRO_INTERNO", ex.Message));
            }
        }
    }
}
