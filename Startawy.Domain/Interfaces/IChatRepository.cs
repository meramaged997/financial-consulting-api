using startawy.Core.Entities;

namespace startawy.Core.Interfaces.Repositories;

public interface IChatRepository : IRepository<ChatSession>
{
    Task<ChatSession?>               GetWithMessagesAsync(int sessionId, CancellationToken ct = default);
    Task<IReadOnlyList<ChatSession>> GetByUserAsync(string userId, CancellationToken ct = default);
    Task<ChatMessage>                AddMessageAsync(ChatMessage message, CancellationToken ct = default);
    Task<int>                        CountUserMessagesAsync(string userId, DateTime sinceUtc, CancellationToken ct = default);
}
