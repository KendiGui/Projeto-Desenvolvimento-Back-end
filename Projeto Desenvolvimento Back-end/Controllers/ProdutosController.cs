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
    [Route("api/[controller]")]
    public class ProdutosController(IProdutosService produtosService) : ControllerBase
    {
        [HttpGet]
        [SwaggerOperation(Summary = "Lista os produtos cadastrados")]
        [ProducesResponseType(typeof(ResultPaginado<ProdutoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> ListaProdutos(int pagina = 1, int tamanhoPagina = 10)
        {
            var result = await produtosService.ListaProdutos(pagina, tamanhoPagina);
            return Ok(result);
        }

        [HttpGet("{produtoId}")]
        [SwaggerOperation(Summary = "Retorna um produto a partir do Id")]
        [ProducesResponseType(typeof(ProdutoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetProduto(long produtoId)
        {
            var result = await produtosService.GetProduto(produtoId);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Gerente}")]
        [SwaggerOperation(Summary = "Cadastra um produto (ADMIN ou GERENTE)")]
        [ProducesResponseType(typeof(ProdutoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult> CadastraProduto([FromBody] ProdutoRequest request)
        {
            var result = await produtosService.CadastraProduto(request);
            return CreatedAtAction(nameof(GetProduto), new { produtoId = result.Id }, result);
        }

        [HttpPut("{produtoId}")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Gerente}")]
        [SwaggerOperation(Summary = "Atualiza um produto (ADMIN ou GERENTE)")]
        [ProducesResponseType(typeof(ProdutoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AtualizaProduto(long produtoId, [FromBody] ProdutoRequest request)
        {
            var result = await produtosService.AtualizaProduto(produtoId, request);
            return Ok(result);
        }

        [HttpDelete("{produtoId}")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Gerente}")]
        [SwaggerOperation(Summary = "Desativa um produto (ADMIN ou GERENTE)")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemoveProduto(long produtoId)
        {
            await produtosService.RemoveProduto(produtoId);
            return NoContent();
        }
    }
}
