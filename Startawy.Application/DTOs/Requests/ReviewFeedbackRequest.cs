using System.ComponentModel.DataAnnotations;

namespace Startawy.Application.DTOs.Requests;

public class ReviewFeedbackRequest
{
    [Required]
    [RegularExpression("^(Suggestion|Complaint|Positive)$", ErrorMessage = "Category must be Suggestion, Complaint, or Positive.")]
    public string Category { get; set; } = "Suggestion";

    public bool MarkReviewed { get; set; } = true;
}

