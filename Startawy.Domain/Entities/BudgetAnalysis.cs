using System;
using System.Collections.Generic;

namespace Startawy.Domain.Entities;

public partial class BudgetAnalysis
{
    public string BudgetId { get; set; } = null!;

    public decimal? VariableCost { get; set; }

    public decimal? FixedCost { get; set; }

    public decimal? Expenses { get; set; }

    public string PackageId { get; set; } = null!;

    public virtual Package Package { get; set; } = null!;
}
