using System.ComponentModel.DataAnnotations;

namespace Startawy.Application.DTOs.Requests;

public class CreateFollowUpPlanRequest
{
    [Required]
    public string FounderUserId { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Goal { get; set; } = string.Empty;

    public DateTime TimelineStartUtc { get; set; } = DateTime.UtcNow;
    public DateTime TimelineEndUtc { get; set; } = DateTime.UtcNow.AddDays(30);

    [Required]
    [MinLength(1)]
    public List<CreateFollowUpStepRequest> Steps { get; set; } = new();
}

public class CreateFollowUpStepRequest
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    public DateTime DueAtUtc { get; set; } = DateTime.UtcNow.AddDays(7);
}

