using Core.Data;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class ItemPedidoRepository(DatabaseContext context) : GenericRepository<ItemPedido>(context), IItemPedidoRepository
    {
    }
}
