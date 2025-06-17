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
    public class EfShippingAddressRepository : IShippingAddressRepository
    {
        private readonly ApplicationDbContext _context;

        public EfShippingAddressRepository(ApplicationDbContext context) => _context = context;

        public async Task<ShippingAddress?> GetByIdAsync(Guid id) =>
            await _context.ShippingAddresses.FindAsync(id);

        public async Task<IEnumerable<ShippingAddress>> ListByUserAsync(Guid userId) =>
            await _context.ShippingAddresses
                .Where(a => a.UserId == userId)
                .ToListAsync();

        public async Task AddAsync(ShippingAddress address)
        {
            await _context.ShippingAddresses.AddAsync(address);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ShippingAddress address)
        {
            _context.ShippingAddresses.Update(address);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.ShippingAddresses.FindAsync(id);
            if (entity != null)
            {
                _context.ShippingAddresses.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
