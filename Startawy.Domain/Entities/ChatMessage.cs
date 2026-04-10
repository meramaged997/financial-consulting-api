namespace startawy.Core.Entities;

public class ChatMessage : BaseEntity
{
    public int         ChatSessionId { get; set; }
    public ChatSession ChatSession   { get; set; } = null!;
    public string      Role          { get; set; } = string.Empty;
    public string      Content       { get; set; } = string.Empty;
    public DateTime    SentAt        { get; set; } = DateTime.UtcNow;
}
