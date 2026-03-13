using System;
using System.Collections.Generic;

namespace Startawy.Domain.Entities;

public partial class Basic
{
    public string PackageId { get; set; } = null!;

    public bool? UnlimitedAi { get; set; }

    public bool? UnlimitedAnalysis { get; set; }

    public virtual Package Package { get; set; } = null!;
}
