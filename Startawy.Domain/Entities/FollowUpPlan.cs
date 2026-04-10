using Startawy.Domain.Entities;

namespace startawy.Core.Entities;

public class FollowUpPlan : AuditableEntity
{
    public string FounderUserId { get; set; } = string.Empty;
    public string ConsultantUserId { get; set; } = string.Empty;
    public User? FounderUser { get; set; }
    public User? ConsultantUser { get; set; }

    public string Goal { get; set; } = string.Empty;
    public DateTime TimelineStartUtc { get; set; } = DateTime.UtcNow;
    public DateTime TimelineEndUtc { get; set; } = DateTime.UtcNow.AddDays(30);

    public ICollection<FollowUpStep> Steps { get; set; } = new List<FollowUpStep>();
}

