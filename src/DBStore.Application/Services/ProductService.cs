using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBStore.Application.Interfaces;
using DBStore.Domain.Contracts;
using DBStore.Domain.Entities;

namespace DBStore.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        public ProductService(IProductRepository repo) => _repo = repo;

        public Task<Product?> GetByIdAsync(Guid id) =>
            _repo.GetByIdAsync(id);

        public Task<IEnumerable<Product>> ListAllAsync() =>
            _repo.ListAllAsync();

        public async Task<Product> CreateAsync(Product product)
        {
            // acá podrías validar stock, nombre, etc.
            await _repo.AddAsync(product);
            return product;
        }

        public Task UpdateAsync(Product product) =>
            _repo.UpdateAsync(product);

        public Task DeleteAsync(Guid id) =>
            _repo.DeleteAsync(id);
    }
}
