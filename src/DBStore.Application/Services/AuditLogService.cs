using System.Collections.Generic;
using System.Threading.Tasks;
using DBStore.Application.Interfaces;
using DBStore.Domain.Contracts;
using DBStore.Domain.Entities;

namespace DBStore.Application.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _repo;
        public AuditLogService(IAuditLogRepository repo) => _repo = repo;

        public Task LogAsync(AuditLog log) =>
            _repo.AddAsync(log);

        public Task<IEnumerable<AuditLog>> ListAllAsync() =>
            _repo.ListAllAsync();
    }
}
