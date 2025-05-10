using System;
using System.Collections.Generic;

namespace EFCoreWebAPI.Models;

public partial class Stock
{
    public int StockIdPk { get; set; }

    public int? ProductIdFk { get; set; }

    public int? StoreIdFk { get; set; }

    public decimal QuantityAvailable { get; set; }

    public DateOnly Date { get; set; }

    public virtual ProductMaster? ProductIdFkNavigation { get; set; }

    public virtual Store? StoreIdFkNavigation { get; set; }
}
