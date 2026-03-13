using Startawy.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Startawy.Domain.Entities;

public partial class Consultant
{
    public string UserId { get; set; } = null!;
    public int? YearsOfExp { get; set; }
    public string? Certificate { get; set; }
    public string? Specialization { get; set; }
    public string? Availability { get; set; }
    public DateOnly? Date { get; set; }
    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
    public virtual User User { get; set; } = null!;
}
