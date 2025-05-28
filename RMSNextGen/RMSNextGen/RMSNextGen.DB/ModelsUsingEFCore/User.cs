using System;
using System.Collections.Generic;

namespace RMSNextGen.Models.ModelsUsingEFCore;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int? RoleId { get; set; }

    public bool IsActive { get; set; }
}
