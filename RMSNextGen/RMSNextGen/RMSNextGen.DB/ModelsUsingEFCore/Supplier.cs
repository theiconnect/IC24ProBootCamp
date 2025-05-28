using System;
using System.Collections.Generic;

namespace RMSNextGen.Models.ModelsUsingEFCore;

public partial class Supplier
{
    public int SupplierIdPk { get; set; }

    public string SupplierCode { get; set; } = null!;

    public int? StatusIdFk { get; set; }

    public string SupplierName { get; set; } = null!;

    public string CompanyName { get; set; } = null!;

    public string? ContactNumber1 { get; set; }

    public string ContactNumber2 { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Gstnumber { get; set; }

    public string? Address { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? LastUpdatedBy { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public virtual StatusMaster? StatusIdFkNavigation { get; set; }

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
