using System;
using System.Collections.Generic;

namespace RMSNextGen.Models.ModelsUsingEFCore;

public partial class ProductMaster
{
    public int ProductIdPk { get; set; }

    public string ProductCode { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public decimal PricePerUnit { get; set; }

    public int? UomidFk { get; set; }

    public decimal ThresholdLimit { get; set; }

    public int? ProductCategoryIdFk { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? LastUpdatedBy { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public virtual ICollection<Billing> Billings { get; set; } = new List<Billing>();

    public virtual ProductCategory? ProductCategoryIdFkNavigation { get; set; }

    public virtual ICollection<StockProduct> StockProducts { get; set; } = new List<StockProduct>();

    public virtual Uom? UomidFkNavigation { get; set; }
}
