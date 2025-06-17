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
    public class EfOrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public EfOrderRepository(ApplicationDbContext context) => _context = context;

        public async Task<Order?> GetByIdAsync(Guid id) =>
            await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

        public async Task<IEnumerable<Order>> ListByUserAsync(Guid userId) =>
            await _context.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == userId)
                .ToListAsync();

        public async Task<IEnumerable<Order>> ListAllAsync() =>
            await _context.Orders
                .Include(o => o.Items)
                .ToListAsync();

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Orders.FindAsync(id);
            if (entity != null)
            {
                _context.Orders.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
