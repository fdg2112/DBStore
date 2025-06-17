using System;
using System.Threading.Tasks;
using DBStore.Domain.Contracts;
using DBStore.Domain.Entities;
using DBStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DBStore.Infrastructure.Repositories
{
    public class EfCartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public EfCartRepository(ApplicationDbContext context) => _context = context;

        public async Task<Cart?> GetByIdAsync(Guid id) =>
            await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Cart?> GetActiveByUserAsync(Guid userId) =>
            await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.IsActive);

        public async Task AddAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Carts.FindAsync(id);
            if (entity != null)
            {
                _context.Carts.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
