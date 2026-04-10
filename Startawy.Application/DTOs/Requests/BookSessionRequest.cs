using System.ComponentModel.DataAnnotations;

namespace Startawy.Application.DTOs.Requests;

public class BookSessionRequest
{
    [Required]
    public int AvailabilitySlotId { get; set; }

    // Session payment is separate from subscription.
    [Required]
    [StringLength(50)]
    public string PaymentMethod { get; set; } = "Card";
}

