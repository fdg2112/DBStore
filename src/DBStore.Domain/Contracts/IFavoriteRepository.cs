namespace DBStore.Domain.Contracts;

﻿using DBStore.Domain.Entities;

public interface IFavoriteRepository
{
    Task<IEnumerable<Favorite>> ListByUserAsync(Guid userId);
    Task<Favorite?> GetByUserAndProductAsync(Guid userId, Guid productId);
    Task AddAsync(Favorite favorite);
    Task DeleteAsync(Guid id);
}