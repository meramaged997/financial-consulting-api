using System.ComponentModel.DataAnnotations;

namespace Startawy.Application.DTOs.Requests;

public class CreateFeedbackRequest
{
    [Required]
    [StringLength(2000)]
    public string Message { get; set; } = string.Empty;
}

