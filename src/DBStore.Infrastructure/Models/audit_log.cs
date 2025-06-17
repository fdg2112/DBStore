using System;
using System.Collections.Generic;

namespace DBStore.Infrastructure.Models;

public partial class audit_log
{
    public Guid id { get; set; }

    public Guid admin_id { get; set; }

    public string table_name { get; set; } = null!;

    public Guid record_id { get; set; }

    public string action { get; set; } = null!;

    public string? changed_data { get; set; }

    public DateTime? created_at { get; set; }

    public virtual user admin { get; set; } = null!;
}
