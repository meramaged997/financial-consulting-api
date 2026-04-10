using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using startawy.Core.Interfaces.Services;

namespace startawy.Infrastructure.Services;

public class OpenAIService : IAIService
{
    private readonly HttpClient _http;
    private readonly string? _apiKey;
    private readonly string _model;

    public OpenAIService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _apiKey = config["OpenAI:ApiKey"];
        _model = config["OpenAI:Model"] ?? "gpt-4o-mini";
    }

    public async Task<string> GetCompletionAsync(
        string systemPrompt,
        string userMessage,
        IEnumerable<(string role, string content)>? history = null,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(_apiKey))
        {
            return "AI is not configured on the server (missing OpenAI:ApiKey). Please ask the admin to set it, or continue using the platform tools (budget/cashflow) for guidance.";
        }

        // OpenAI chat completions (minimal, provider-agnostic)
        var messages = new List<Dictionary<string, string>>
        {
            new() { ["role"] = "system", ["content"] = systemPrompt }
        };

        if (history is not null)
        {
            foreach (var (role, content) in history)
            {
                var normalized = role.Equals("Assistant", StringComparison.OrdinalIgnoreCase) ? "assistant"
                               : role.Equals("User", StringComparison.OrdinalIgnoreCase) ? "user"
                               : role.ToLowerInvariant();
                messages.Add(new() { ["role"] = normalized, ["content"] = content });
            }
        }

        messages.Add(new() { ["role"] = "user", ["content"] = userMessage });

        var payload = new
        {
            model = _model,
            messages,
            temperature = 0.3,
            max_tokens = 600
        };

        using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        using var res = await _http.SendAsync(req, ct);
        var json = await res.Content.ReadAsStringAsync(ct);
        if (!res.IsSuccessStatusCode)
            return $"AI request failed ({(int)res.StatusCode}). Please try again later.";

        try
        {
            using var doc = JsonDocument.Parse(json);
            var content = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();
            return string.IsNullOrWhiteSpace(content) ? "AI returned an empty response." : content.Trim();
        }
        catch
        {
            return "AI response parsing failed. Please try again later.";
        }
    }
}
