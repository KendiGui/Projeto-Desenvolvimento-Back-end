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
    [Authorize]
    [Route("api/[controller]")]
    public class PedidosController(IPedidoService pedidoService) : ControllerBase
    {
        [HttpPost]
        [SwaggerOperation(Summary = "Cria um pedido e processa o pagamento (mock)")]
        [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult> CriarPedido([FromBody] CriarPedidoRequest request)
        {
            var result = await pedidoService.CriarPedido(request, User.GetUserId(), HttpContext.GetIp());
            return CreatedAtAction(nameof(GetPedido), new { pedidoId = result.PedidoId }, result);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Lista pedidos (cliente vê apenas os próprios). Filtros: canalPedido, status")]
        [ProducesResponseType(typeof(ResultPaginado<PedidoResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult> ListaPedidos(
            [FromQuery] string? canalPedido,
            [FromQuery] string? status,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10)
        {
            var result = await pedidoService.ListaPedidos(canalPedido, status, User.GetUserId(), User.GetUserRole(), pagina, tamanhoPagina);
            return Ok(result);
        }

        [HttpGet("{pedidoId}")]
        [SwaggerOperation(Summary = "Retorna um pedido pelo Id")]
        [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetPedido(long pedidoId)
        {
            var result = await pedidoService.GetPedido(pedidoId, User.GetUserId(), User.GetUserRole());
            return Ok(result);
        }

        [HttpPatch("{pedidoId}/status")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Gerente},{Roles.Cozinha},{Roles.Atendente}")]
        [SwaggerOperation(Summary = "Altera o status do pedido respeitando as transições permitidas")]
        [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> AtualizarStatus(long pedidoId, [FromBody] AtualizarStatusPedidoRequest request)
        {
            var result = await pedidoService.AtualizarStatus(pedidoId, request, User.GetUserId(), User.GetUserRole(), HttpContext.GetIp());
            return Ok(result);
        }

        [HttpPost("{pedidoId}/cancelar")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Gerente},{Roles.Atendente}")]
        [SwaggerOperation(Summary = "Cancela um pedido (ADMIN, GERENTE ou ATENDENTE)")]
        [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Cancelar(long pedidoId)
        {
            var result = await pedidoService.CancelarPedido(pedidoId, User.GetUserId(), User.GetUserRole(), HttpContext.GetIp());
            return Ok(result);
        }
    }
}
