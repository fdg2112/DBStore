using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBStore.Domain.Contracts;
using DBStore.Domain.Entities;
using DBStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DBStore.Infrastructure.Repositories
{
    public class EfCartItemRepository : ICartItemRepository
    {
        private readonly ApplicationDbContext _context;

        public EfCartItemRepository(ApplicationDbContext context) => _context = context;

        public async Task<CartItem?> GetByIdAsync(Guid id) =>
            await _context.CartItems.FindAsync(id);

        public async Task<IEnumerable<CartItem>> ListByCartAsync(Guid cartId) =>
            await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();

        public async Task AddAsync(CartItem item)
        {
            await _context.CartItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CartItem item)
        {
            _context.CartItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.CartItems.FindAsync(id);
            if (entity != null)
            {
                _context.CartItems.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
