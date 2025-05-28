using System;
using System.Collections.Generic;

namespace RMSNextGen.Models.ModelsUsingEFCore;

public partial class Uom
{
    public int UomidPk { get; set; }

    public string Uomcode { get; set; } = null!;

    public string Uom1 { get; set; } = null!;

    public string Uomdescription { get; set; } = null!;

    public int? ProductCategoryIdFk { get; set; }

    public virtual ProductCategory? ProductCategoryIdFkNavigation { get; set; }

    public virtual ICollection<ProductMaster> ProductMasters { get; set; } = new List<ProductMaster>();
}
