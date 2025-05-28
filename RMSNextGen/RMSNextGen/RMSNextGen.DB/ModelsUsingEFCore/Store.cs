using System;
using System.Collections.Generic;

namespace RMSNextGen.Models.ModelsUsingEFCore;

public partial class Store
{
    public int StoreIdpk { get; set; }

    public string StoreCode { get; set; } = null!;

    public int? StatusIdFk { get; set; }

    public string StoreName { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Fax { get; set; }

    public string? Gst { get; set; }

    public string? Cin { get; set; }

    public string ManagerName { get; set; } = null!;

    public string ManagerContactNumber { get; set; } = null!;

    public bool IsCorporateOffice { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? LastUpdatedBy { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public virtual ICollection<Billing> Billings { get; set; } = new List<Billing>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual StatusMaster? StatusIdFkNavigation { get; set; }

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
