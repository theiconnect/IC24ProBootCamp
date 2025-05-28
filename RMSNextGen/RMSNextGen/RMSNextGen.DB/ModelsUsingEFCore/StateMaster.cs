using System;
using System.Collections.Generic;

namespace RMSNextGen.Models.ModelsUsingEFCore;

public partial class StateMaster
{
    public int StateId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<CityMaster> CityMasters { get; set; } = new List<CityMaster>();
}
