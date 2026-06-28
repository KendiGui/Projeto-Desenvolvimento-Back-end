using Domain.Contracts;
using Domain.Contracts.Requests;
using Domain.Contracts.Responses;
using Domain.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Projeto_Desenvolvimento_Back_end.Controllers
{
    [ApiController]
    [Route("api/unidades")]
    public class CardapioController(ICardapioService cardapioService) : ControllerBase
    {
        [HttpGet("{unidadeId}/cardapio")]
        [SwaggerOperation(Summary = "Lista o cardápio de uma unidade")]
        [ProducesResponseType(typeof(IEnumerable<CardapioItemResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetCardapio(long unidadeId)
        {
            var result = await cardapioService.GetCardapio(unidadeId);
            return Ok(result);
        }

        [HttpPost("{unidadeId}/produtos")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Gerente}")]
        [SwaggerOperation(Summary = "Adiciona um produto ao cardápio da unidade (ADMIN ou GERENTE)")]
        [ProducesResponseType(typeof(CardapioItemResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult> AdicionaProduto(long unidadeId, [FromBody] CardapioProdutoRequest request)
        {
            var result = await cardapioService.AdicionaProduto(unidadeId, request);
            return CreatedAtAction(nameof(GetCardapio), new { unidadeId }, result);
        }

        [HttpPut("{unidadeId}/produtos/{produtoId}")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Gerente}")]
        [SwaggerOperation(Summary = "Atualiza um produto no cardápio da unidade (ADMIN ou GERENTE)")]
        [ProducesResponseType(typeof(CardapioItemResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AtualizaProduto(long unidadeId, long produtoId, [FromBody] CardapioProdutoRequest request)
        {
            var result = await cardapioService.AtualizaProduto(unidadeId, produtoId, request);
            return Ok(result);
        }
    }
}
