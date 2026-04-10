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

    // Payment workflow (production-like)
    public string Status { get; set; } = "Succeeded"; // Pending, Succeeded, Failed
    public string? IdempotencyKey { get; set; }
    public string? ExternalReference { get; set; }
    public string? ReferenceType { get; set; } // e.g. Package, Session
    public string? ReferenceId { get; set; }   // e.g. package_id or session_id

    public string UserId { get; set; } = null!;

    public string? SubsId { get; set; }

    public virtual Subscription? Subs { get; set; }

    public virtual User User { get; set; } = null!;
}
