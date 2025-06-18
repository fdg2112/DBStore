using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBStore.Domain.Entities;

namespace DBStore.Application.Interfaces
{
    public interface IShippingAddressService
    {
        Task<IEnumerable<ShippingAddress>> ListByUserAsync(Guid userId);
        Task<ShippingAddress> AddAsync(ShippingAddress address);
        Task UpdateAsync(ShippingAddress address);
        Task DeleteAsync(Guid id, Guid userId);
    }
}
