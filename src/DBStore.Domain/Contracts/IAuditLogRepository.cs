using DBStore.Domain.Entities;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog log);
    Task<IEnumerable<AuditLog>> ListAllAsync();      // para admin
}