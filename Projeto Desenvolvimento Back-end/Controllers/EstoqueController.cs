using Domain.Contracts;
using Domain.Contracts.Requests;
using Domain.Contracts.Responses;
using Domain.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_Desenvolvimento_Back_end.Configurations;
using Service.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Projeto_Desenvolvimento_Back_end.Controllers
{
    [ApiController]
    [Route("api")]
    public class EstoqueController(IEstoqueService estoqueService) : ControllerBase
    {
        [HttpGet("unidades/{unidadeId}/estoque")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Gerente},{Roles.Atendente}")]
        [SwaggerOperation(Summary = "Consulta o saldo de estoque de uma unidade")]
        [ProducesResponseType(typeof(IEnumerable<EstoqueResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetEstoque(long unidadeId)
        {
            var result = await estoqueService.GetEstoquePorUnidade(unidadeId);
            return Ok(result);
        }

        [HttpPost("estoque/movimentos")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Gerente}")]
        [SwaggerOperation(Summary = "Registra um movimento de estoque (ADMIN ou GERENTE)")]
        [ProducesResponseType(typeof(MovimentoEstoqueResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult> CriarMovimento([FromBody] EstoqueMovimentoRequest request)
        {
            var result = await estoqueService.CriarMovimento(request, User.GetUserId(), HttpContext.GetIp());
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet("estoque/movimentos")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Gerente},{Roles.Atendente}")]
        [SwaggerOperation(Summary = "Lista movimentos de estoque, com filtros por unidade e produto")]
        [ProducesResponseType(typeof(IEnumerable<MovimentoEstoqueResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult> ListaMovimentos([FromQuery] long? unidadeId, [FromQuery] long? produtoId)
        {
            var result = await estoqueService.ListaMovimentos(unidadeId, produtoId);
            return Ok(result);
        }
    }
}
