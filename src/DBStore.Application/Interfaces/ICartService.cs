using System;
using System.Threading.Tasks;
using DBStore.Domain.Entities;

namespace DBStore.Application.Interfaces
{
    public interface ICartService
    {
        Task<Cart> GetOrCreateActiveCartAsync(Guid userId);
        Task AddItemAsync(Guid cartId, Guid productId, int quantity);
        Task RemoveItemAsync(Guid itemId);
        Task CheckoutAsync(Guid cartId);
    }
}
