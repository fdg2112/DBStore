using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBStore.Domain.Entities;

namespace DBStore.Application.Interfaces
{
    public interface IFavoriteService
    {
        Task<IEnumerable<Favorite>> ListByUserAsync(Guid userId);
        Task ToggleFavoriteAsync(Guid userId, Guid productId);
    }
}
