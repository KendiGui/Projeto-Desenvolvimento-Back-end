using Core.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Context;
using Domain.Repositories;

namespace Infrastructure.Repositories
{
    public class UsuarioRepository(DatabaseContext context) : GenericRepository<Usuario>(context), IUsuarioRepository
    {
        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}
