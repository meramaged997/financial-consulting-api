using Startawy.Application.DTOs.Requests;
using startawy.Application.Common.Models;
using startawy.Application.DTOs.Responses;

namespace Startawy.Application.Interfaces;

public interface IChatService
{
    Task<Result<ChatMessageResponse>> SendMessageAsync(string userId, SendChatMessageRequest request, CancellationToken ct = default);
    Task<Result<IReadOnlyList<ChatSessionResponse>>> GetSessionsAsync(string userId, CancellationToken ct = default);
    Task<Result<IReadOnlyList<ChatHistoryItemResponse>>> GetHistoryAsync(string userId, int sessionId, CancellationToken ct = default);
}
