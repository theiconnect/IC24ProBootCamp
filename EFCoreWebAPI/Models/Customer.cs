using System;
using System.Collections.Generic;

namespace EFCoreWebAPI.Models;

public partial class Customer
{
    public int CustomerIdPk { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string ContactNumber { get; set; } = null!;

    public string CustomerCode { get; set; } = null!;
}
