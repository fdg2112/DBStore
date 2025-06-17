using System;
using System.Collections.Generic;

namespace DBStore.Infrastructure.Models;

public partial class user
{
    public Guid id { get; set; }

    public string email { get; set; } = null!;

    public string password_hash { get; set; } = null!;

    public bool? email_verified { get; set; }

    public string? password_reset_token { get; set; }

    public string? avatar_url { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public virtual ICollection<audit_log> audit_logs { get; set; } = new List<audit_log>();

    public virtual ICollection<cart> carts { get; set; } = new List<cart>();

    public virtual ICollection<favorite> favorites { get; set; } = new List<favorite>();

    public virtual ICollection<order> ordercreated_byNavigations { get; set; } = new List<order>();

    public virtual ICollection<order> orderupdated_byNavigations { get; set; } = new List<order>();

    public virtual ICollection<order> orderusers { get; set; } = new List<order>();

    public virtual ICollection<product> productcreated_byNavigations { get; set; } = new List<product>();

    public virtual ICollection<product> productupdated_byNavigations { get; set; } = new List<product>();

    public virtual ICollection<shipping_address> shipping_addresses { get; set; } = new List<shipping_address>();

    public virtual ICollection<role> roles { get; set; } = new List<role>();
}
