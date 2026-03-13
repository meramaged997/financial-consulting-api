using System;
using System.Collections.Generic;

namespace Startawy.Domain.Entities;

public partial class StartupFounder
{
    public string UserId { get; set; } = null!;

    public string? BusinessName { get; set; }

    public string? BusinessSector { get; set; }

    public string? Description { get; set; }

    public DateOnly? FoundingDate { get; set; }

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

    public virtual ICollection<StartawyReport> StartawyReports { get; set; } = new List<StartawyReport>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();
}
