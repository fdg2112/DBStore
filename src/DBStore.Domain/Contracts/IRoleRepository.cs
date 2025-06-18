namespace DBStore.Domain.Contracts;

﻿using DBStore.Domain.Entities;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(Guid id);
    Task<Role?> GetByNameAsync(string name);
    Task<IEnumerable<Role>> ListAllAsync();
    Task AddAsync(Role role);
    Task DeleteAsync(Guid id);
}