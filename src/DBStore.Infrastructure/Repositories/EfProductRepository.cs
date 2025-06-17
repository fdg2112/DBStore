using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBStore.Domain.Contracts;
using DBStore.Domain.Entities;
using DBStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DBStore.Infrastructure.Repositories
{
    public class EfProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public EfProductRepository(ApplicationDbContext context) => _context = context;

        public async Task<Product?> GetByIdAsync(Guid id) =>
            await _context.Products.FindAsync(id);

        public async Task<IEnumerable<Product>> ListAllAsync() =>
            await _context.Products.ToListAsync();

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Products.FindAsync(id);
            if (entity != null)
            {
                _context.Products.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
