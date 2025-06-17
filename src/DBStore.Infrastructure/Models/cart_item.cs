using System;
using System.Collections.Generic;

namespace DBStore.Infrastructure.Models;

public partial class cart_item
{
    public Guid id { get; set; }

    public Guid cart_id { get; set; }

    public Guid product_id { get; set; }

    public int? quantity { get; set; }

    public virtual cart cart { get; set; } = null!;

    public virtual product product { get; set; } = null!;
}
