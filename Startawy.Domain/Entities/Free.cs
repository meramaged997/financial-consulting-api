using System;

namespace Startawy.Domain.Entities;

public partial class Free
{
    public string PackageId { get; set; } = null!;
    public int? FreeTrial { get; set; }
    public virtual Package Package { get; set; } = null!;
}
