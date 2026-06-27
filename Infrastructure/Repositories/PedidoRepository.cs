using Core.Data;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class PedidoRepository(DatabaseContext context) : GenericRepository<Pedido>(context), IPedidoRepository
    {
    }
}
