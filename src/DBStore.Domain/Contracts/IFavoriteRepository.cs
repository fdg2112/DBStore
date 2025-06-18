using DBStore.Domain.Entities;

namespace DBStore.Domain.Contracts
{
    public interface IFavoriteRepository
    {
        Task<IEnumerable<Favorite>> ListByUserAsync(Guid userId);
        Task<Favorite?> GetByUserAndProductAsync(Guid userId, Guid productId);
        Task AddAsync(Favorite favorite);
        Task DeleteAsync(Guid id);
    }
}
