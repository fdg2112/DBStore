namespace DBStore.Domain.Contracts;

﻿using DBStore.Domain.Entities;

public interface ICartItemRepository
{
    Task<CartItem?> GetByIdAsync(Guid id);
    Task<IEnumerable<CartItem>> ListByCartAsync(Guid cartId);
    Task AddAsync(CartItem item);
    Task UpdateAsync(CartItem item);
    Task DeleteAsync(Guid id);
}