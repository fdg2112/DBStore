using System;
using System.Collections.Generic;

namespace DBStore.Infrastructure.Models;

public partial class favorite
{
    public Guid id { get; set; }

    public Guid user_id { get; set; }

    public Guid product_id { get; set; }

    public virtual product product { get; set; } = null!;

    public virtual user user { get; set; } = null!;
}
