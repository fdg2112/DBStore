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
    public class EfOrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;

        public EfOrderItemRepository(ApplicationDbContext context) => _context = context;

        public async Task<OrderItem?> GetByIdAsync(Guid id) =>
            await _context.OrderItems.FindAsync(id);

        public async Task<IEnumerable<OrderItem>> ListByOrderAsync(Guid orderId) =>
            await _context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();

        public async Task AddAsync(OrderItem item)
        {
            await _context.OrderItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(OrderItem item)
        {
            _context.OrderItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.OrderItems.FindAsync(id);
            if (entity != null)
            {
                _context.OrderItems.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
