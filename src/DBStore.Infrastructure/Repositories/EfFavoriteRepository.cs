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
    public class EfFavoriteRepository : IFavoriteRepository
    {
        private readonly ApplicationDbContext _context;

        public EfFavoriteRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Favorite>> ListByUserAsync(Guid userId) =>
            await _context.Favorites
                .Where(f => f.UserId == userId)
                .ToListAsync();

        public async Task<Favorite?> GetByUserAndProductAsync(Guid userId, Guid productId) =>
            await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

        public async Task AddAsync(Favorite favorite)
        {
            await _context.Favorites.AddAsync(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Favorites.FindAsync(id);
            if (entity != null)
            {
                _context.Favorites.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
