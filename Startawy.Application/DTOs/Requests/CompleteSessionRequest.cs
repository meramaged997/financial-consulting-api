using System.ComponentModel.DataAnnotations;

namespace Startawy.Application.DTOs.Requests;

public class CompleteSessionRequest
{
    [Required]
    public string Notes { get; set; } = string.Empty;

    [Required]
    public string Recommendations { get; set; } = string.Empty;
}

