using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBStore.Application.Interfaces;
using DBStore.Domain.Contracts;
using DBStore.Domain.Entities;

namespace DBStore.Application.Services
{
    public class ShippingAddressService : IShippingAddressService
    {
        private readonly IShippingAddressRepository _repo;
        public ShippingAddressService(IShippingAddressRepository repo) => _repo = repo;

        public Task<IEnumerable<ShippingAddress>> ListByUserAsync(Guid userId) =>
            _repo.ListByUserAsync(userId);

        public async Task<ShippingAddress> AddAsync(ShippingAddress address)
        {
            await _repo.AddAsync(address);
            return address;
        }

        public Task UpdateAsync(ShippingAddress address) =>
            _repo.UpdateAsync(address);

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null || existing.UserId != userId)
                throw new InvalidOperationException("Dirección inválida");
            await _repo.DeleteAsync(id);
        }
    }
}
