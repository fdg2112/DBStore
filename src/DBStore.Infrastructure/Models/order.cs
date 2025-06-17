using System;
using System.Collections.Generic;

namespace DBStore.Infrastructure.Models;

public partial class order
{
    public Guid id { get; set; }

    public Guid user_id { get; set; }

    public Guid? billing_address_id { get; set; }

    public string payment_status { get; set; } = null!;

    public string shipping_status { get; set; } = null!;

    public string? transaction_id { get; set; }

    public decimal total { get; set; }

    public Guid? created_by { get; set; }

    public Guid? updated_by { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public virtual shipping_address? billing_address { get; set; }

    public virtual user? created_byNavigation { get; set; }

    public virtual ICollection<order_item> order_items { get; set; } = new List<order_item>();

    public virtual user? updated_byNavigation { get; set; }

    public virtual user user { get; set; } = null!;
}
