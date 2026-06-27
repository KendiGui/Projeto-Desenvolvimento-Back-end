using Core.Data;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class ProdutoRepository(DatabaseContext context) : GenericRepository<Produto>(context), IProdutoRepository
    {
    }
}
