using Domain.Contracts.Responses;
using Domain.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Projeto_Desenvolvimento_Back_end.Controllers
{
    [ApiController]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Gerente}")]
    [Route("api/[controller]")]
    public class AuditoriasController(IAuditoriaService auditoriaService) : ControllerBase
    {
        [HttpGet]
        [SwaggerOperation(Summary = "Lista os registros de auditoria (ADMIN ou GERENTE). Filtros: entidade, entidadeId")]
        [ProducesResponseType(typeof(ResultPaginado<AuditoriaResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Lista(
            [FromQuery] string? entidade,
            [FromQuery] long? entidadeId,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10)
        {
            var result = await auditoriaService.ListarAsync(entidade, entidadeId, pagina, tamanhoPagina);
            return Ok(result);
        }
    }
}
