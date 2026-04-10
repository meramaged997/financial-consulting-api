using Startawy.Domain.Entities;

namespace startawy.Core.Entities;

public class ConsultationSession : AuditableEntity
{
    public string FounderUserId { get; set; } = string.Empty;
    public string ConsultantUserId { get; set; } = string.Empty;
    public User? FounderUser { get; set; }
    public User? ConsultantUser { get; set; }

    public DateTime StartAtUtc { get; set; }
    public DateTime EndAtUtc { get; set; }

    public string Status { get; set; } = "Booked"; // Booked, Completed, Cancelled

    public decimal Fee { get; set; }
    public string? PaymentTransactionId { get; set; }

    public string? ConsultantNotes { get; set; }
    public string? ConsultantRecommendations { get; set; }
}

