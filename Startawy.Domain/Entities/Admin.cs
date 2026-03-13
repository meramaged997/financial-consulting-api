using System;
using System.Collections.Generic;

namespace Startawy.Domain.Entities;

public partial class Admin
{
    public string UserId { get; set; } = null!;

    public string AdminLevel { get; set; } = null!;

    public string? AccessScope { get; set; }

    public virtual User User { get; set; } = null!;
}
