using Core.Data;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class MovimentoEstoqueRepository(DatabaseContext context) : GenericRepository<MovimentoEstoque>(context), IMovimentoEstoqueRepository
    {
    }
}
