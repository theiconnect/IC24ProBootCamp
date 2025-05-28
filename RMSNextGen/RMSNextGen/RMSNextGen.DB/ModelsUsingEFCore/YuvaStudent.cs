using System;
using System.Collections.Generic;

namespace RMSNextGen.Models.ModelsUsingEFCore;

public partial class YuvaStudent
{
    public string StudentCode { get; set; } = null!;

    public string StudentName { get; set; } = null!;

    public DateOnly Dob { get; set; }

    public string Grade { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public bool IsOwnTransport { get; set; }

    public string Comments { get; set; } = null!;
}
