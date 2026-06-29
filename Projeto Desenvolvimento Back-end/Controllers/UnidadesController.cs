using Domain.Contracts;
using Domain.Contracts.Requests;
using Domain.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Projeto_Desenvolvimento_Back_end.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnidadesController(IUnidadesService unidadesService) : ControllerBase
    {
        [HttpGet]
        [SwaggerOperation(Summary = "Lista as unidades cadastradas")]
        [ProducesResponseType(typeof(ResultPaginado<UnidadeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ListaUnidades([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 10)
        {
            var result = await unidadesService.ListaUnidades(pagina, tamanhoPagina);
            return Ok(result);
        }

        [HttpGet("{unidadeId}")]
        [SwaggerOperation(Summary = "Retorna uma unidade a partir do Id")]
        [ProducesResponseType(typeof(UnidadeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetUnidade(long unidadeId)
        {
            var result = await unidadesService.GetUnidade(unidadeId);
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cadastra uma unidade")]
        [ProducesResponseType(typeof(UnidadeResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CadastraUnidade(UnidadeRequest request)
        {
            var result = await unidadesService.CadastraUnidade(request);
            return CreatedAtAction(nameof(GetUnidade), result);
        }

        [HttpPost("{unidadeId}")]
        [SwaggerOperation(Summary = "Cadastra uma unidade")]
        [ProducesResponseType(typeof(UnidadeResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AtualizaUnidade([FromBody] UnidadeRequest request, long unidadeId)
        {
            var result = await unidadesService.AtualizaUnidade(unidadeId, request);
            return Ok(result);
        }
    }
}
