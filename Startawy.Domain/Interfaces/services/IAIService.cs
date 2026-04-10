namespace startawy.Core.Interfaces.Services;

public interface IAIService
{
    Task<string> GetCompletionAsync(
        string systemPrompt,
        string userMessage,
        IEnumerable<(string role, string content)>? history = null,
        CancellationToken ct = default);
}
