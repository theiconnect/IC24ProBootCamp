using System;
using System.Collections.Generic;

namespace EFCoreWebAPI.Models;

public partial class Store
{
    public int StoreIdPk { get; set; }

    public string StoreCode { get; set; } = null!;

    public string StoreName { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string? ManagerName { get; set; }

    public string StoreContactNumber { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
