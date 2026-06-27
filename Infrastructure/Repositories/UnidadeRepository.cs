using Core.Data;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class UnidadeRepository(DatabaseContext context) : GenericRepository<Unidade>(context), IUnidadeRepository
    {
    }
}
