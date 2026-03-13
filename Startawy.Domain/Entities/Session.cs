using System;
using System.Collections.Generic;

namespace Startawy.Domain.Entities;

public partial class Session
{
    public string SessionId { get; set; } = null!;
    public DateOnly Date { get; set; }
    public string? Notes { get; set; }
    public string FounderId { get; set; } = null!;
    public string ConsultantId { get; set; } = null!;
    public virtual Consultant Consultant { get; set; } = null!;
    public virtual StartupFounder Founder { get; set; } = null!;
}
