using System;
using System.Collections.Generic;

namespace Startawy.Domain.Entities;

public partial class ChatBot
{
    public string ChatId { get; set; } = null!;

    public int? ChatLimit { get; set; }

    public DateTime Time { get; set; }

    public string? SysResponse { get; set; }

    public string? UserMessage { get; set; }

    public string UserId { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
