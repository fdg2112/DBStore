namespace DBStore.Domain.Contracts;

﻿using DBStore.Domain.Entities;

public interface IShippingAddressRepository
{
    Task<ShippingAddress?> GetByIdAsync(Guid id);
    Task<IEnumerable<ShippingAddress>> ListByUserAsync(Guid userId);
    Task AddAsync(ShippingAddress address);
    Task UpdateAsync(ShippingAddress address);
    Task DeleteAsync(Guid id);
}
