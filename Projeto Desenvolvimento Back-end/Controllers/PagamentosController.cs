using Domain.Contracts;
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
    [Route("api")]
    public class PagamentosController(IPagamentoService pagamentoService) : ControllerBase
    {
        [HttpGet("pedidos/{pedidoId}/pagamento")]
        [SwaggerOperation(Summary = "Retorna o pagamento de um pedido")]
        [ProducesResponseType(typeof(PagamentoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetPagamento(long pedidoId)
        {
            var result = await pagamentoService.GetPagamentoDoPedido(pedidoId, User.GetUserId(), User.GetUserRole());
            return Ok(result);
        }
    }
}
