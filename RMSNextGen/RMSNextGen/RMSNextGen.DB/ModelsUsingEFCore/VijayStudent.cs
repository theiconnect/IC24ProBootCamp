using System;
using System.Collections.Generic;

namespace RMSNextGen.Models.ModelsUsingEFCore;

public partial class VijayStudent
{
    public string StudentCode { get; set; } = null!;

    public string StudentName { get; set; } = null!;

    public string Dob { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string Grade { get; set; } = null!;

    public string IsOwnTransport { get; set; } = null!;

    public string Comments { get; set; } = null!;
}
