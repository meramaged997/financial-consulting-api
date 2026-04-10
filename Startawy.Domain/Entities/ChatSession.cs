namespace startawy.Core.Entities;

public class ChatSession : BaseEntity
{
    public string   UserId        { get; set; } = string.Empty;
    public string   Title         { get; set; } = string.Empty;
    public DateTime LastMessageAt { get; set; } = DateTime.UtcNow;

    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
}
