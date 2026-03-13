using System;
using System.Collections.Generic;

namespace Startawy.Domain.Entities;

public partial class StartawyReport
{
    public string ReportId { get; set; } = null!;

    public string? Title { get; set; }

    public string? Industry { get; set; }

    public string? Link { get; set; }

    public DateOnly? UploadDate { get; set; }

    public string FounderId { get; set; } = null!;

    public virtual StartupFounder Founder { get; set; } = null!;
}
