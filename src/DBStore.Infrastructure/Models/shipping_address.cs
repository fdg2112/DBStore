using System;
using System.Collections.Generic;

namespace DBStore.Infrastructure.Models;

public partial class shipping_address
{
    public Guid id { get; set; }

    public Guid user_id { get; set; }

    public string address_line { get; set; } = null!;

    public string? city { get; set; }

    public string? state { get; set; }

    public string? zip_code { get; set; }

    public string? country { get; set; }

    public string shipping_method { get; set; } = null!;

    public decimal cost { get; set; }

    public string? carrier { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public virtual ICollection<order> orders { get; set; } = new List<order>();

    public virtual user user { get; set; } = null!;
}
