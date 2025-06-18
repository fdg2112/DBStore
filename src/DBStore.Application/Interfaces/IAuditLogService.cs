using System.Collections.Generic;
using System.Threading.Tasks;
using DBStore.Domain.Entities;

namespace DBStore.Application.Interfaces
{
    public interface IAuditLogService
    {
        Task LogAsync(AuditLog log);
        Task<IEnumerable<AuditLog>> ListAllAsync();
    }
}
