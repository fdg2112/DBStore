using DBStore.Domain.Entities;
namespace DBStore.Domain.Contracts
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid id);
        Task<IEnumerable<Order>> ListByUserAsync(Guid userId);
        Task<IEnumerable<Order>> ListAllAsync();         // para admin
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Guid id);
    }
}
