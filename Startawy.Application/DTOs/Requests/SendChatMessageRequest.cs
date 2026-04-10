namespace Startawy.Application.DTOs.Requests;

public record SendChatMessageRequest(string Message, int? SessionId);
