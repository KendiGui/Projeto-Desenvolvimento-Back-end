using Core.Data;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class UnidadeProdutoRepository(DatabaseContext context) : GenericRepository<UnidadeProduto>(context), IUnidadeProdutoRepository
    {
    }
}
