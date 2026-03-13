using System;
using System.Collections.Generic;

namespace Startawy.Domain.Entities;

public partial class Premium
{
    public string PackageId { get; set; } = null!;

    public int? FollowUpDuration { get; set; }

    public bool? ConsultantReview { get; set; }

    public bool? ConsultantSupport { get; set; }

    public virtual Package Package { get; set; } = null!;
}
