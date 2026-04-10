using startawy.Core.Entities;
using startawy.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Startawy.Infrastructure.Data;
using Startawy.Infrastructure.Repositories;

namespace startawy.Infrastructure.Repositories;

public class ChatRepository : Repository<ChatSession>, IChatRepository
{
    public ChatRepository(AppDbContext db) : base(db) { }

    public async Task<ChatSession?> GetWithMessagesAsync(int sessionId, CancellationToken ct = default)
        => await _db.ChatSessions.Include(s => s.Messages.OrderBy(m => m.SentAt)).FirstOrDefaultAsync(s => s.Id == sessionId, ct);

    public async Task<IReadOnlyList<ChatSession>> GetByUserAsync(string userId, CancellationToken ct = default)
        => await _db.ChatSessions.Include(s => s.Messages).Where(s => s.UserId == userId).OrderByDescending(s => s.LastMessageAt).ToListAsync(ct);

    public async Task<ChatMessage> AddMessageAsync(ChatMessage message, CancellationToken ct = default)
    {
        await _db.ChatMessages.AddAsync(message, ct);
        await _db.SaveChangesAsync(ct);
        return message;
    }

    public async Task<int> CountUserMessagesAsync(string userId, DateTime sinceUtc, CancellationToken ct = default)
        => await _db.ChatMessages
            .Where(m => m.ChatSession.UserId == userId && m.Role == "User" && m.SentAt >= sinceUtc)
            .CountAsync(ct);
}
