using System;
using System.Collections.Generic;

namespace RMSNextGen.Models.ModelsUsingEFCore;

public partial class Order
{
    public int OrderIdpk { get; set; }

    public string InvoiceNumber { get; set; } = null!;

    public string OrderName { get; set; } = null!;

    public int? StoreIdFk { get; set; }

    public int? CustomerIdFk { get; set; }

    public int? StatusIdFk { get; set; }

    public DateOnly OrderDate { get; set; }

    public decimal? NoOfItems { get; set; }

    public decimal? TotalAmount { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? LastUpdatedBy { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public virtual ICollection<Billing> Billings { get; set; } = new List<Billing>();

    public virtual Customer? CustomerIdFkNavigation { get; set; }

    public virtual StatusMaster? StatusIdFkNavigation { get; set; }

    public virtual Store? StoreIdFkNavigation { get; set; }
}
