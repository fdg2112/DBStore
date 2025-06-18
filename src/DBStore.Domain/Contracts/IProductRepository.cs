namespace DBStore.Domain.Contracts;

﻿using DBStore.Domain.Entities;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> ListAllAsync();
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Guid id);
}