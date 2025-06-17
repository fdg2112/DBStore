using System;
using System.Collections.Generic;

namespace DBStore.Infrastructure.Models;

public partial class cart
{
    public Guid id { get; set; }

    public Guid user_id { get; set; }

    public bool? is_active { get; set; }

    public DateTime? expires_at { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public virtual ICollection<cart_item> cart_items { get; set; } = new List<cart_item>();

    public virtual user user { get; set; } = null!;
}
