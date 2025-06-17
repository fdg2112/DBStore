using System;
using System.Collections.Generic;

namespace DBStore.Infrastructure.Models;

public partial class product
{
    public Guid id { get; set; }

    public string name { get; set; } = null!;

    public string? description { get; set; }

    public decimal price { get; set; }

    public int? stock { get; set; }

    public string? image_url { get; set; }

    public Guid? created_by { get; set; }

    public Guid? updated_by { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public virtual ICollection<cart_item> cart_items { get; set; } = new List<cart_item>();

    public virtual user? created_byNavigation { get; set; }

    public virtual ICollection<favorite> favorites { get; set; } = new List<favorite>();

    public virtual ICollection<order_item> order_items { get; set; } = new List<order_item>();

    public virtual user? updated_byNavigation { get; set; }
}
