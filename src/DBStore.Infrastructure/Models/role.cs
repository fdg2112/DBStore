﻿using System;
using System.Collections.Generic;

namespace DBStore.Infrastructure.Models;

public partial class role
{
    public Guid id { get; set; }

    public string name { get; set; } = null!;

    public virtual ICollection<user> users { get; set; } = new List<user>();
}
