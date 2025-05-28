using System;
using System.Collections.Generic;

namespace RMSNextGen.Models.ModelsUsingEFCore;

public partial class Role
{
    public int RoleIdPk { get; set; }

    public string RoleCode { get; set; } = null!;

    public string RoleName { get; set; } = null!;

    public string? Description { get; set; }
}
