using System;
using System.Collections.Generic;

namespace RMSNextGen.Models.ModelsUsingEFCore;

public partial class Employee
{
    public int EmployeeIdPk { get; set; }

    public string EmployeeCode { get; set; } = null!;

    public string EmployeeFirstName { get; set; } = null!;

    public string? EmployeeLastName { get; set; }

    public string Email { get; set; } = null!;

    public string MobileNumber { get; set; } = null!;

    public int? StoreIdFk { get; set; }

    public int? StatusIdFk { get; set; }

    public string? Department { get; set; }

    public string? Designation { get; set; }

    public string? PersonalEmail { get; set; }

    public string Gender { get; set; } = null!;

    public decimal? SalaryCtc { get; set; }

    public string PermanentAddressLine1 { get; set; } = null!;

    public string? PermanentAddressLine2 { get; set; }

    public string PermanentCity { get; set; } = null!;

    public string PermanentState { get; set; } = null!;

    public string? PermanentPinCode { get; set; }

    public string CurrentAddressLine1 { get; set; } = null!;

    public string? CurrentAddressLine2 { get; set; }

    public string CurrentCity { get; set; } = null!;

    public string CurrentState { get; set; } = null!;

    public string? CurrentPinCode { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? LastUpdatedBy { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public virtual StatusMaster? StatusIdFkNavigation { get; set; }

    public virtual Store? StoreIdFkNavigation { get; set; }
}
