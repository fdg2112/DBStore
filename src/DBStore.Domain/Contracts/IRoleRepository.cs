﻿using DBStore.Domain.Entities;
namespace DBStore.Domain.Contracts
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(Guid id);
        Task<Role?> GetByNameAsync(string name);
        Task<IEnumerable<Role>> ListAllAsync();
        Task AddAsync(Role role);
        Task DeleteAsync(Guid id);
    }
}
