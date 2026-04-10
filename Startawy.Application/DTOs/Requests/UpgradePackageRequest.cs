using System.ComponentModel.DataAnnotations;

namespace Startawy.Application.DTOs.Requests;

public class UpgradePackageRequest
{
    [Required]
    public string PackageId { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string PaymentMethod { get; set; } = "Card";
}
