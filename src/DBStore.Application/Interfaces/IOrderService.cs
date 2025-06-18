using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBStore.Domain.Entities;

namespace DBStore.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Guid userId, Guid billingAddressId);
        Task<Order?> GetByIdAsync(Guid id, Guid userId);
        Task<IEnumerable<Order>> ListByUserAsync(Guid userId);
        Task<IEnumerable<Order>> ListAllAsync();  // sólo admin
    }
}
