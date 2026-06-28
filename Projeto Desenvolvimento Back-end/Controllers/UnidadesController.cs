using Domain.Contracts;
using Domain.Contracts.Exceptions;
using Domain.Contracts.Requests;
using Domain.Contracts.Responses;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Interfaces;
using Service.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Projeto_Desenvolvimento_Back_end.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnidadesController(IUnidadesService unidadesService, ILogger<UnidadesController> logger) : ControllerBase
    {
        [HttpGet]
        [SwaggerOperation(Summary = "Lista as unidades cadastradas")]
        [ProducesResponseType(typeof(ResultPaginado<UnidadeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> ListaUnidades(int pagina, int tamanhoPagina)
        {
            try
            {
                var result = await unidadesService.ListaUnidades(pagina, tamanhoPagina);
                return Ok(result);
            }
            catch (EmptyListException)
            {
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro interno");
                return StatusCode(500, new ErroResponse("ERRO_INTERNO", ex.Message));
            }
        }

        [HttpGet("{unidadeId}")]
        [SwaggerOperation(Summary = "Retorna uma unidade a partir do Id")]
        [ProducesResponseType(typeof(UnidadeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetUnidade(long unidadeId)
        {
            try
            {
                var result = await unidadesService.GetUnidade(unidadeId);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ErroResponse("NAO_ENCONTRADO", ex.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro interno");
                return StatusCode(500, new ErroResponse("ERRO_INTERNO", ex.Message));
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cadastra uma unidade")]
        [ProducesResponseType(typeof(UnidadeResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CadastraUnidade(UnidadeRequest request)
        {
            try
            {
                var result = await unidadesService.CadastraUnidade(request);
                return CreatedAtAction(nameof(GetUnidade), result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro interno");
                return StatusCode(500, new ErroResponse("ERRO_INTERNO", ex.Message));
            }
        }

        [HttpPost("{unidadeId}")]
        [SwaggerOperation(Summary = "Cadastra uma unidade")]
        [ProducesResponseType(typeof(UnidadeResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AtualizaUnidade([FromBody] UnidadeRequest request, long unidadeId)
        {
            try
            {
                var result = await unidadesService.AtualizaUnidade(unidadeId, request);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ErroResponse("NAO_ENCONTRADO", ex.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro interno");
                return StatusCode(500, new ErroResponse("ERRO_INTERNO", ex.Message));
            }
        }
    }
}
