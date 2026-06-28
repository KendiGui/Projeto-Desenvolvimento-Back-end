using Domain.Contracts;
using Domain.Contracts.Requests;
using Domain.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_Desenvolvimento_Back_end.Configurations;
using Service.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Projeto_Desenvolvimento_Back_end.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class FidelidadeController(IFidelidadeService fidelidadeService) : ControllerBase
    {
        [HttpGet("saldo")]
        [SwaggerOperation(Summary = "Consulta o saldo de pontos do cliente autenticado")]
        [ProducesResponseType(typeof(FidelidadeSaldoResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetSaldo()
        {
            var result = await fidelidadeService.GetSaldo(User.GetUserId());
            return Ok(result);
        }

        [HttpGet("historico")]
        [SwaggerOperation(Summary = "Consulta o histórico de pontos do cliente autenticado")]
        [ProducesResponseType(typeof(IEnumerable<FidelidadeHistoricoResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetHistorico()
        {
            var result = await fidelidadeService.GetHistorico(User.GetUserId());
            return Ok(result);
        }

        [HttpPost("resgatar")]
        [SwaggerOperation(Summary = "Resgata pontos de fidelidade")]
        [ProducesResponseType(typeof(FidelidadeSaldoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult> Resgatar([FromBody] ResgatarPontosRequest request)
        {
            var result = await fidelidadeService.Resgatar(User.GetUserId(), request);
            return Ok(result);
        }

        [HttpPut("consentimento")]
        [SwaggerOperation(Summary = "Registra o consentimento de marketing (LGPD)")]
        [ProducesResponseType(typeof(FidelidadeSaldoResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult> AtualizarConsentimento([FromBody] ConsentimentoRequest request)
        {
            var result = await fidelidadeService.AtualizarConsentimento(User.GetUserId(), request);
            return Ok(result);
        }
    }
}
