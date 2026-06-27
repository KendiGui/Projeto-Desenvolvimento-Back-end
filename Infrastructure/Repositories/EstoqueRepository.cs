using Core.Data;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class EstoqueRepository(DatabaseContext context) : GenericRepository<Estoque>(context), IEstoqueRepository
    {
    }
}
