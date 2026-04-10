using Startawy.Domain.Entities;

namespace startawy.Core.Entities;

public class ConsultantAvailabilitySlot : BaseEntity
{
    public string ConsultantUserId { get; set; } = string.Empty;
    public User? ConsultantUser { get; set; }
    public DateTime StartAtUtc { get; set; }
    public DateTime EndAtUtc { get; set; }

    public bool IsBooked { get; set; }
    public int? ConsultationSessionId { get; set; }
    public ConsultationSession? ConsultationSession { get; set; }
}

