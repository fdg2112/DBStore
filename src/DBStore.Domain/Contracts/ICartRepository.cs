using DBStore.Domain.Entities;

public interface ICartRepository
{
    Task<Cart?> GetByIdAsync(Guid id);
    Task<Cart?> GetActiveByUserAsync(Guid userId);
    Task AddAsync(Cart cart);
    Task UpdateAsync(Cart cart);
    Task DeleteAsync(Guid id);
}