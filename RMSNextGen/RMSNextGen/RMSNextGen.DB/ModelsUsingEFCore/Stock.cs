using System;
using System.Collections.Generic;

namespace RMSNextGen.Models.ModelsUsingEFCore;

public partial class Stock
{
    public int StockIdPk { get; set; }

    public string StockCode { get; set; } = null!;

    public int PurchaseOrderNumber { get; set; }

    public string InvoiceNumber { get; set; } = null!;

    public int? StoreIdFk { get; set; }

    public int? SuplierIdFk { get; set; }

    public int? StatusIdFk { get; set; }

    public DateTime StockInTime { get; set; }

    public string? Remarks { get; set; }

    public string? VehicleNumber { get; set; }

    public string? ApprovedBy { get; set; }

    public DateTime ApprovedOn { get; set; }

    public string? ApprovedComments { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? LastUpdatedBy { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public virtual StatusMaster? StatusIdFkNavigation { get; set; }

    public virtual ICollection<StockProduct> StockProducts { get; set; } = new List<StockProduct>();

    public virtual Store? StoreIdFkNavigation { get; set; }

    public virtual Supplier? SuplierIdFkNavigation { get; set; }
}
