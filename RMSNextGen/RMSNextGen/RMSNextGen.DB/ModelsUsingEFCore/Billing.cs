using System;
using System.Collections.Generic;

namespace RMSNextGen.Models.ModelsUsingEFCore;

public partial class Billing
{
    public int BillingIdPk { get; set; }

    public int OrderIdFk { get; set; }

    public string BillingCode { get; set; } = null!;

    public int? StoreIdFk { get; set; }

    public int? ProductIdFk { get; set; }

    public int? StatusIdFk { get; set; }

    public decimal Quantity { get; set; }

    public decimal PricePerUnit { get; set; }

    public string InvoiceNumber { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public virtual Order OrderIdFkNavigation { get; set; } = null!;

    public virtual ProductMaster? ProductIdFkNavigation { get; set; }

    public virtual StatusMaster? StatusIdFkNavigation { get; set; }

    public virtual Store? StoreIdFkNavigation { get; set; }
}
