using startawy.Core.Entities;
using startawy.Core.Interfaces.Repositories;
using startawy.Application.Common.Models;
using Startawy.Application.DTOs.Requests;
using startawy.Application.DTOs.Responses;
using Startawy.Application.Interfaces;
using Startawy.Domain.Interfaces;
using startawy.Core.Interfaces.Services;

namespace Startawy.Application.Services;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepo;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IAIService _aiService;

    private const int FreeMonthlyMessageLimit = 20;

    public ChatService(IChatRepository chatRepo, ISubscriptionRepository subscriptionRepository, IAIService aiService)
    {
        _chatRepo = chatRepo;
        _subscriptionRepository = subscriptionRepository;
        _aiService = aiService;
    }

    public async Task<Result<ChatMessageResponse>> SendMessageAsync(string userId, SendChatMessageRequest request, CancellationToken ct = default)
    {
        var activeSub = await _subscriptionRepository.GetActiveByUserAsync(userId, ct);
        var packageType = activeSub?.Package?.Type ?? "Free";

        // Business rule: Free users have limited AI requests; paid users unlimited.
        if (packageType.Equals("Free", StringComparison.OrdinalIgnoreCase))
        {
            var since = DateTime.UtcNow.AddDays(-30);
            var used = await _chatRepo.CountUserMessagesAsync(userId, since, ct);
            if (used >= FreeMonthlyMessageLimit)
                return Result<ChatMessageResponse>.Failure($"AI chat limit reached for Free plan ({FreeMonthlyMessageLimit} messages per 30 days). Upgrade to Basic/Premium for unlimited access.");
        }

        ChatSession session;
        if (request.SessionId.HasValue)
        {
            var existing = await _chatRepo.GetByIdAsync(request.SessionId.Value, ct);
            if (existing is null) return Result<ChatMessageResponse>.Failure("Session not found.");
            if (existing.UserId != userId) return Result<ChatMessageResponse>.Failure("Not authorized.");
            session = existing;
        }
        else
        {
            session = await _chatRepo.AddAsync(new ChatSession
            {
                UserId = userId,
                Title = "Chat",
                LastMessageAt = DateTime.UtcNow
            }, ct);
        }

        var userMessage = new ChatMessage
        {
            ChatSessionId = session.Id,
            Role = "User",
            Content = request.Message,
            SentAt = DateTime.UtcNow
        };
        await _chatRepo.AddMessageAsync(userMessage, ct);

        // Build limited history for context (most recent messages).
        var historySession = await _chatRepo.GetWithMessagesAsync(session.Id, ct);
        var history = (historySession?.Messages ?? Array.Empty<ChatMessage>())
            .OrderByDescending(m => m.SentAt)
            .Take(12)
            .OrderBy(m => m.SentAt)
            .Select(m => (role: m.Role, content: m.Content))
            .ToList();

        var systemPrompt =
            "You are Startawy AI, a financial consulting assistant for startup founders. " +
            "Give practical, actionable advice. If numbers are missing, ask concise clarifying questions. " +
            "Avoid legal/medical advice. Keep answers structured with short bullet points.\n\n" +
            $"User plan: {packageType}.";

        var reply = await _aiService.GetCompletionAsync(systemPrompt, request.Message, history, ct);
        var assistantMessage = new ChatMessage
        {
            ChatSessionId = session.Id,
            Role = "Assistant",
            Content = reply,
            SentAt = DateTime.UtcNow
        };
        await _chatRepo.AddMessageAsync(assistantMessage, ct);

        return Result<ChatMessageResponse>.Success(new ChatMessageResponse(
            session.Id, reply, new List<string>(), DateTime.UtcNow
        ));
    }

    public async Task<Result<IReadOnlyList<ChatSessionResponse>>> GetSessionsAsync(string userId, CancellationToken ct = default)
    {
        var list = await _chatRepo.GetByUserAsync(userId, ct);
        var response = list.Select(s => new ChatSessionResponse(
            s.Id, s.Title, s.LastMessageAt, s.Messages?.Count ?? 0
        )).ToList();
        return Result<IReadOnlyList<ChatSessionResponse>>.Success(response);
    }

    public async Task<Result<IReadOnlyList<ChatHistoryItemResponse>>> GetHistoryAsync(string userId, int sessionId, CancellationToken ct = default)
    {
        var session = await _chatRepo.GetWithMessagesAsync(sessionId, ct);
        if (session is null) return Result<IReadOnlyList<ChatHistoryItemResponse>>.Failure("Session not found.");
        if (session.UserId != userId) return Result<IReadOnlyList<ChatHistoryItemResponse>>.Failure("Not authorized.");
        var list = (session.Messages ?? Array.Empty<ChatMessage>())
            .OrderBy(m => m.SentAt)
            .Select(m => new ChatHistoryItemResponse(m.Id, m.Role, m.Content, m.SentAt))
            .ToList();
        return Result<IReadOnlyList<ChatHistoryItemResponse>>.Success(list);
    }
}
