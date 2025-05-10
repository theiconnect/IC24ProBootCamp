using System;
using System.Collections.Generic;

namespace EFCoreWebAPI.Models;

public partial class Order
{
    public int OrderIdPk { get; set; }

    public DateOnly OrderDate { get; set; }

    public int StoreIdFk { get; set; }

    public int EmployeeIdFk { get; set; }

    public int CustomerIdFk { get; set; }

    public string OrderCode { get; set; } = null!;

    public int? NoOfItems { get; set; }

    public decimal TotalAmount { get; set; }

    public virtual Customer1 CustomerIdFkNavigation { get; set; } = null!;

    public virtual Employee EmployeeIdFkNavigation { get; set; } = null!;

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual Store StoreIdFkNavigation { get; set; } = null!;
}
