using System;
using System.Collections.Generic;

namespace RMSNextGen.Models.ModelsUsingEFCore;

public partial class StatusMaster
{
    public int StatusIdPk { get; set; }

    public string StatusCode { get; set; } = null!;

    public string StatusName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Billing> Billings { get; set; } = new List<Billing>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<StockProduct> StockProducts { get; set; } = new List<StockProduct>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();

    public virtual ICollection<Store> Stores { get; set; } = new List<Store>();

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}
