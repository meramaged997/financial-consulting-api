using System.ComponentModel.DataAnnotations;

namespace Startawy.Application.DTOs.Package;

public class CreatePackageRequest
{
    [Required]
    [StringLength(100)]
    public string Type { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? DurationDays { get; set; }

    // Basic / Premium extras (optional)
    public bool? UnlimitedAi { get; set; }
    public bool? UnlimitedAnalysis { get; set; }
    public int? FollowUpDurationDays { get; set; }
    public bool? ConsultantReview { get; set; }
    public bool? ConsultantSupport { get; set; }
}