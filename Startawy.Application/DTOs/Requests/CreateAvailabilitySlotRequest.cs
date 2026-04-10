using System.ComponentModel.DataAnnotations;

namespace Startawy.Application.DTOs.Requests;

public class CreateAvailabilitySlotRequest
{
    [Required]
    public DateTime StartAtUtc { get; set; }

    [Required]
    public DateTime EndAtUtc { get; set; }
}

