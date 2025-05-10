using System;
using System.Collections.Generic;

namespace EFCoreWebAPI.Models;

public partial class ProductMaster
{
    public int ProductIdPk { get; set; }

    public string? ProductCode { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal PricePerUnit { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
