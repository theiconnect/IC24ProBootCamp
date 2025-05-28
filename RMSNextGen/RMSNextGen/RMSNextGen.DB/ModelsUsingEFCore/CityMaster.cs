using System;
using System.Collections.Generic;

namespace RMSNextGen.Models.ModelsUsingEFCore;

public partial class CityMaster
{
    public int CityId { get; set; }

    public string Name { get; set; } = null!;

    public int StateId { get; set; }

    public virtual StateMaster State { get; set; } = null!;
}
