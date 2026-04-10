namespace Startawy.Application.DTOs.Package;

public class PackageResponse
{
    public string PackageId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? DurationDays { get; set; }

    // Basic features
    public bool? UnlimitedAi { get; set; }
    public bool? UnlimitedAnalysis { get; set; }

    // Premium features
    public int? FollowUpDurationDays { get; set; }
    public bool? ConsultantReview { get; set; }
    public bool? ConsultantSupport { get; set; }
}
