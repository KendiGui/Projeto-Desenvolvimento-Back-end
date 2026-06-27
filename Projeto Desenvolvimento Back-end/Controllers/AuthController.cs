using System.Security.Claims;
using Domain.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Projeto_Desenvolvimento_Back_end.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
    {

        /// <summary>
        /// Registra um novo usuário no sistema
        /// </summary>
        /// <param name="request">Dados do novo usuário</param>
        /// <returns>Token JWT e dados do usuário registrado</returns>
        /// <response code="201">Usuário registrado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="409">Email já registrado</response>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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
                return Conflict(new { error = "EMAIL_DUPLICADO", message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning("Dados inválidos: {Message}", ex.Message);
                return BadRequest(new { error = "VALIDATION_ERROR", message = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao registrar usuário");
                return StatusCode(500, new { error = "INTERNAL_ERROR", message = "Erro ao registrar usuário" });
            }
        }

        /// <summary>
        /// Realiza login do usuário
        /// </summary>
        /// <param name="request">Credenciais do usuário</param>
        /// <returns>Token JWT e dados do usuário</returns>
        /// <response code="200">Login realizado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="401">Credenciais inválidas</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
                return BadRequest(new { error = "VALIDATION_ERROR", message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.LogWarning("Falha na autenticação: {Message}", ex.Message);
                return Unauthorized(new { error = "UNAUTHORIZED", message = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao fazer login");
                return StatusCode(500, new { error = "INTERNAL_ERROR", message = "Erro ao fazer login" });
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetMe()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!long.TryParse(userIdClaim, out var userId))
                {
                    logger.LogWarning("Falha ao extrair ID do usuário do token");
                    return Unauthorized(new { error = "UNAUTHORIZED", message = "Token inválido" });
                }

                var result = await authService.GetMeAsync(userId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                logger.LogWarning("Usuário não encontrado: {Message}", ex.Message);
                return NotFound(new { error = "NOT_FOUND", message = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao obter dados do usuário");
                return StatusCode(500, new { error = "INTERNAL_ERROR", message = "Erro ao obter dados do usuário" });
            }
        }
    }
}
