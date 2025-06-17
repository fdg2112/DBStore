using System.Collections.Generic;
using System.Threading.Tasks;
using DBStore.Domain.Contracts;
using DBStore.Domain.Entities;
using DBStore.Infrastructure.Data;

namespace DBStore.Infrastructure.Repositories
{
    public class EfAuditLogRepository : IAuditLogRepository
    {
        private readonly ApplicationDbContext _context;

        public EfAuditLogRepository(ApplicationDbContext context) => _context = context;

        public async Task AddAsync(AuditLog log)
        {
            await _context.AuditLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuditLog>> ListAllAsync() =>
            await _context.AuditLogs.ToListAsync();
    }
}
