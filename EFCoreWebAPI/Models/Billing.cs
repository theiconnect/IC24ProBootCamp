using System;
using System.Collections.Generic;

namespace EFCoreWebAPI.Models;

public partial class Billing
{
    public int BillIdPk { get; set; }

    public string BillNumber { get; set; } = null!;

    public int OrderIdFk { get; set; }

    public string PaymentMode { get; set; } = null!;

    public DateOnly BillingDate { get; set; }

    public decimal Amount { get; set; }
}
