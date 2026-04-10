namespace Startawy.API.Tests;

using System.Net.Http.Json;
using System.Net.Http.Headers;

public class UnitTest1
{
    [Fact]
    public async Task Register_ReturnsValidationErrors_WhenPayloadInvalid()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();

        var res = await client.PostAsJsonAsync("/api/auth/register", new { });
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, res.StatusCode);

        var body = await res.Content.ReadAsStringAsync();
        Assert.Contains("Validation failed", body);
    }

    [Fact]
    public async Task Register_CreatesUser_AndDuplicateEmailReturnsConflict()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();

        var payload = new
        {
            fullName = "Test User",
            email = "test@example.com",
            password = "Aa1!aaaa",
            confirmPassword = "Aa1!aaaa",
            phoneNumber = "01000000000",
            role = 1 // StartupFounder
        };

        var first = await client.PostAsJsonAsync("/api/auth/register", payload);
        Assert.Equal(System.Net.HttpStatusCode.Created, first.StatusCode);

        var second = await client.PostAsJsonAsync("/api/auth/register", payload);
        Assert.Equal(System.Net.HttpStatusCode.Conflict, second.StatusCode);
    }

    [Fact]
    public async Task ProtectedEndpoints_Return401_WithStandardEnvelope_WhenMissingToken()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();

        var res = await client.GetAsync("/api/Budget");
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, res.StatusCode);

        var body = await res.Content.ReadAsStringAsync();
        Assert.Contains("\"success\":false", body);
        Assert.Contains("Unauthorized", body);
    }

    [Fact]
    public async Task ProtectedEndpoints_Return200_WithStandardEnvelope_WhenTokenProvided()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();

        // Register to get a JWT
        var payload = new
        {
            fullName = "Budget User",
            email = $"budget{Guid.NewGuid():N}@example.com",
            password = "Aa1!aaaa",
            confirmPassword = "Aa1!aaaa",
            phoneNumber = "01000000000",
            role = 1 // StartupFounder
        };

        var reg = await client.PostAsJsonAsync("/api/auth/register", payload);
        reg.EnsureSuccessStatusCode();

        var regJson = await reg.Content.ReadFromJsonAsync<ApiEnvelope<AuthData>>(new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(regJson);
        Assert.True(regJson!.Success);
        Assert.NotNull(regJson.Data);
        Assert.False(string.IsNullOrWhiteSpace(regJson.Data!.Token));

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", regJson.Data.Token);

        var budget = await client.GetAsync("/api/Budget");
        Assert.Equal(System.Net.HttpStatusCode.OK, budget.StatusCode);

        var body = await budget.Content.ReadAsStringAsync();
        Assert.Contains("\"success\":true", body);
    }
}

public sealed class ApiEnvelope<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public string[]? Errors { get; set; }
}

public sealed class AuthData
{
    public string Token { get; set; } = string.Empty;
}
