using System;
using System.Collections.Generic;

namespace RMSNextGen.Models.ModelsUsingEFCore;

public partial class StockProduct
{
    public int StockProductIdPk { get; set; }

    public int StockIdFk { get; set; }

    public int ProductIdFk { get; set; }

    public int? StatusIdFk { get; set; }

    public decimal RecievedQuantity { get; set; }

    public decimal PricePerUnit { get; set; }

    public decimal AvailableQuantity { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? LastUpdatedBy { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public virtual ProductMaster ProductIdFkNavigation { get; set; } = null!;

    public virtual StatusMaster? StatusIdFkNavigation { get; set; }

    public virtual Stock StockIdFkNavigation { get; set; } = null!;
}
