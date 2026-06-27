using Core.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Context;
using Domain.Repositories;

namespace Infrastructure.Repositories
{
    public class UsuarioRepository(DatabaseContext context) : GenericRepository<Usuario>(context), IUsuarioRepository
    {
    }
}
