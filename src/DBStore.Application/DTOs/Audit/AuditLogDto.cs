using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBStore.Application.DTOs.Audit
{
    public class AuditLogDto
    {
        public Guid Id { get; set; }
        public Guid AdminId { get; set; }
        public string TableName { get; set; } = null!;
        public string Action { get; set; } = null!;
        public string? ChangedData { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
