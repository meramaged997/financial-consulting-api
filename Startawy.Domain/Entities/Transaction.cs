using System;
using System.Collections.Generic;

namespace Startawy.Domain.Entities;

public partial class Transaction
{
    public string TransId { get; set; } = null!;

    public DateOnly TransDate { get; set; }

    public decimal Amount { get; set; }

    public string Type { get; set; } = null!;

    public string? PaymentMethod { get; set; }

    public string UserId { get; set; } = null!;

    public string? SubsId { get; set; }

    public virtual Subscription? Subs { get; set; }

    public virtual User User { get; set; } = null!;
}
