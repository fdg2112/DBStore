using DBStore.Domain.Entities;
namespace DBStore.Domain.Contracts
{
    public interface IShippingAddressRepository
    {
        Task<ShippingAddress?> GetByIdAsync(Guid id);
        Task<IEnumerable<ShippingAddress>> ListByUserAsync(Guid userId);
        Task AddAsync(ShippingAddress address);
        Task UpdateAsync(ShippingAddress address);
        Task DeleteAsync(Guid id);
    }
}

