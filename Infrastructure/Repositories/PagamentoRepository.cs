using Core.Data;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class PagamentoRepository(DatabaseContext context) : GenericRepository<Pagamento>(context), IPagamentoRepository
    {
    }
}
