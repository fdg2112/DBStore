using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBStore.Domain.Entities;

namespace DBStore.Application.Interfaces
{
    public interface IProductService
    {
        Task<Product?> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> ListAllAsync();
        Task<Product> CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Guid id);
    }
}
