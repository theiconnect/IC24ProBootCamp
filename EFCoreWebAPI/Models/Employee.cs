using System;
using System.Collections.Generic;

namespace EFCoreWebAPI.Models;

public partial class Employee
{
    public int EmployeeIdPk { get; set; }

    public string EmployeeName { get; set; } = null!;

    public string? Role { get; set; }

    public DateOnly? DateOfJoining { get; set; }

    public string ContactNumber { get; set; } = null!;

    public int? StoreIdFk { get; set; }

    public string EmployeeCode { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string Salary { get; set; } = null!;

    public DateOnly? DateOfLeaving { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Store? StoreIdFkNavigation { get; set; }
}
