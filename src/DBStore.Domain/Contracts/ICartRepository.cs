using DBStore.Domain.Entities;

namespace DBStore.Domain.Contracts
{
    public interface ICartRepository
    {
        Task<Cart?> GetByIdAsync(Guid id);
        Task<Cart?> GetActiveByUserAsync(Guid userId);
        Task AddAsync(Cart cart);
        Task UpdateAsync(Cart cart);
        Task DeleteAsync(Guid id);
    }
}
