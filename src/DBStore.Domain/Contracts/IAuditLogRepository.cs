using DBStore.Domain.Entities;
namespace DBStore.Domain.Contracts
{
    public interface IAuditLogRepository
    {
        Task AddAsync(AuditLog log);
        Task<IEnumerable<AuditLog>> ListAllAsync();      // para admin
    }
}