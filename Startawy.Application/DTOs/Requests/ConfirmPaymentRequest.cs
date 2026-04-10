using System.ComponentModel.DataAnnotations;

namespace Startawy.Application.DTOs.Requests;

public class ConfirmPaymentRequest
{
    public bool Succeeded { get; set; } = true;

    [StringLength(200)]
    public string? ExternalReference { get; set; }
}

