namespace startawy.Application.DTOs.Responses;

public record ChatMessageResponse(
    int      SessionId,
    string   Reply,
    List<string> SuggestedFollowUps,
    DateTime Timestamp
);

public record ChatSessionResponse(
    int      Id,
    string   Title,
    DateTime LastMessageAt,
    int      MessageCount
);

public record ChatHistoryItemResponse(
    int      Id,
    string   Role,
    string   Content,
    DateTime SentAt
);
