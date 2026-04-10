using System.ComponentModel.DataAnnotations;

namespace Startawy.Application.DTOs.Requests;

public class CreatePaymentIntentRequest
{
    [Required]
    [RegularExpression("^(SubscriptionUpgrade|SessionBooking)$")]
    public string Purpose { get; set; } = "SubscriptionUpgrade";

    // For SubscriptionUpgrade
    public string? PackageId { get; set; }

    // For SessionBooking
    public int? AvailabilitySlotId { get; set; }

    [Required]
    [StringLength(50)]
    public string PaymentMethod { get; set; } = "Card";
}

