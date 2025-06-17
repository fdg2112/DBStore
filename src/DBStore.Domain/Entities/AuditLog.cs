using System;

namespace DBStore.Domain.Entities
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        public Guid AdminId { get; set; }
        public string TableName { get; set; } = null!;
        public Guid RecordId { get; set; }
        public string Action { get; set; } = null!;
        public string? ChangedData { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
