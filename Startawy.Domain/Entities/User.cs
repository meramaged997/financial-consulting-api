using System;
using System.Collections.Generic;

namespace Startawy.Domain.Entities;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public string Type { get; set; } = null!;

    public virtual Admin? Admin { get; set; }

    public virtual ICollection<ChatBot> ChatBots { get; set; } = new List<ChatBot>();

    public virtual Consultant? Consultant { get; set; }

    public virtual StartupFounder? StartupFounder { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
