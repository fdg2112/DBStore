using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBStore.Domain.Contracts;
using DBStore.Domain.Entities;
using DBStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DBStore.Infrastructure.Repositories
{
    public class EfRoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public EfRoleRepository(ApplicationDbContext context) => _context = context;

        public async Task<Role?> GetByIdAsync(Guid id) =>
            await _context.Roles.FindAsync(id);

        public async Task<Role?> GetByNameAsync(string name) =>
            await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);

        public async Task<IEnumerable<Role>> ListAllAsync() =>
            await _context.Roles.ToListAsync();

        public async Task AddAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Roles.FindAsync(id);
            if (entity != null)
            {
                _context.Roles.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
