using Startawy.Domain.Entities;

namespace startawy.Core.Entities;

public class Feedback : AuditableEntity
{
    public string UserId { get; set; } = string.Empty;
    public User? User { get; set; }
    public string Message { get; set; } = string.Empty;

    public string Category { get; set; } = "Uncategorized"; // Suggestion, Complaint, Positive
    public bool IsReviewed { get; set; }
    public string? ReviewedByAdminId { get; set; }
    public DateTime? ReviewedAtUtc { get; set; }
}

