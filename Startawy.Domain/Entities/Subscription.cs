using System;
using System.Collections.Generic;

namespace Startawy.Domain.Entities;

public partial class Subscription
{
    public string SubsId { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? TrialType { get; set; }

    public string UserId { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User User { get; set; } = null!;
}
