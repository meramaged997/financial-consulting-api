using startawy.Core.Enums;

namespace startawy.Core.Entities;

public class ConsultationRequest : AuditableEntity
{
    public string             UserId           { get; set; } = string.Empty;
    public string             Subject          { get; set; } = string.Empty;
    public string             Description      { get; set; } = string.Empty;
    public ConsultationType   Type             { get; set; }
    public ConsultationStatus Status           { get; set; } = ConsultationStatus.Pending;
    public string?            AssignedExpertId { get; set; }
    public string?            ExpertNotes      { get; set; }
    public string             PreAnalysis      { get; set; } = string.Empty;
    public DateTime           RequestedAt      { get; set; } = DateTime.UtcNow;
    public DateTime?          ScheduledAt      { get; set; }
    public DateTime?          CompletedAt      { get; set; }
}
