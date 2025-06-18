using DBStore.Domain.Entities;
namespace DBStore.Domain.Contracts
{
    public interface IOrderItemRepository
    {
        Task<OrderItem?> GetByIdAsync(Guid id);
        Task<IEnumerable<OrderItem>> ListByOrderAsync(Guid orderId);
        Task AddAsync(OrderItem item);
        Task UpdateAsync(OrderItem item);
        Task DeleteAsync(Guid id);
    }
}
