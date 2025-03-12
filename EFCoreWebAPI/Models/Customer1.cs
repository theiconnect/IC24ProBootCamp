using System;
using System.Collections.Generic;

namespace EFCoreWebAPI.Models;

public partial class Customer1
{
    public int CustomerIdPk { get; set; }

    public string CustomerCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
