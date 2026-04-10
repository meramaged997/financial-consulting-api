namespace startawy.Core.Entities;

public abstract class AuditableEntity : BaseEntity
{
    public string  CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }
}
