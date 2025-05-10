using System;
using System.Collections.Generic;

namespace EFCoreWebAPI.Models;

public partial class OrderProduct
{
    public int OrderProductIdPk { get; set; }

    public int? OrderIdFk { get; set; }

    public int? ProductIdFk { get; set; }

    public string Quantity { get; set; } = null!;

    public string PriceUnit { get; set; } = null!;

    public string Amount { get; set; } = null!;

    public virtual Order? OrderIdFkNavigation { get; set; }

    public virtual ProductMaster? ProductIdFkNavigation { get; set; }
}
