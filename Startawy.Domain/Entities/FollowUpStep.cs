namespace startawy.Core.Entities;

public class FollowUpStep : BaseEntity
{
    public int FollowUpPlanId { get; set; }
    public FollowUpPlan FollowUpPlan { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DueAtUtc { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, InProgress, Done
}

