using System;
using System.Collections.Generic;

namespace Startawy.Domain.Entities;

public partial class Package
{
    public string PackageId { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? Duration { get; set; }

    public virtual Free? Free { get; set; }
    public virtual Basic? Basic { get; set; }

    public virtual ICollection<BudgetAnalysis> BudgetAnalyses { get; set; } = new List<BudgetAnalysis>();

    public virtual Premium? Premium { get; set; }

    public virtual ICollection<StartupFounder> Founders { get; set; } = new List<StartupFounder>();
}
