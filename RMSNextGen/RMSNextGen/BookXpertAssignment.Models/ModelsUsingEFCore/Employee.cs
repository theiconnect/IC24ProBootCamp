using System;
using System.Collections.Generic;

namespace BookXpertAssignment.Models.ModelsUsingEFCore;

public partial class Employee
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Designation { get; set; }

    public DateOnly? DateOfJoin { get; set; }

    public decimal? Salary { get; set; }

    public string? Gender { get; set; }

    public int? StateId { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public virtual State? State { get; set; }
}
