using System;
using System.Collections.Generic;

namespace BookXpertAssignment.Models.ModelsUsingEFCore;

public partial class State
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
