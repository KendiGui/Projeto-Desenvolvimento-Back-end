using Core.Data;
using Domain.Contracts.Responses;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Infrastructure.Context;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PedidoRepository(DatabaseContext context) : GenericRepository<Pedido>(context), IPedidoRepository
    {
        public async Task<Pedido?> GetCompletoAsync(long id)
        {
            return await _dbSet
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                .Include(p => p.Pagamento)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ResultPaginado<Pedido>> ListFiltradoAsync(
            long? clienteId,
            CanalPedidoEnum? canalPedido,
            StatusPedidoEnum? status,
            int pagina,
            int tamanhoPagina)
        {
            var query = _dbSet
                .AsNoTracking()
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                .Include(p => p.Pagamento)
            .AsQueryable();

            if (clienteId.HasValue)
                query = query.Where(p => p.ClienteId == clienteId.Value);

            if (canalPedido.HasValue)
                query = query.Where(p => p.CanalPedido == canalPedido.Value);

            if (status.HasValue)
                query = query.Where(p => p.Status == status.Value);

            return await query
                .OrderByDescending(p => p.CreatedAt)
                .ToResultPaginadoAsync(pagina, tamanhoPagina);
        }
    }
}
