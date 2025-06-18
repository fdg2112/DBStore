using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBStore.Application.Interfaces;
using DBStore.Domain.Contracts;
using DBStore.Domain.Entities;

namespace DBStore.Application.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _repo;
        public FavoriteService(IFavoriteRepository repo) => _repo = repo;

        public Task<IEnumerable<Favorite>> ListByUserAsync(Guid userId) =>
            _repo.ListByUserAsync(userId);

        public async Task ToggleFavoriteAsync(Guid userId, Guid productId)
        {
            var existing = await _repo.GetByUserAndProductAsync(userId, productId);
            if (existing != null)
                await _repo.DeleteAsync(existing.Id);
            else
                await _repo.AddAsync(new Favorite { UserId = userId, ProductId = productId });
        }
    }
}
