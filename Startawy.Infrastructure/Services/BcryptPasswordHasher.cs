using Startawy.Application.Interfaces;

namespace Startawy.Infrastructure.Services;

/// <summary>
/// BCrypt password hashing (work factor 12).
/// Install: dotnet add package BCrypt.Net-Next
/// </summary>
public class BcryptPasswordHasher : IPasswordHasher
{
    private const int WorkFactor = 12;

    public string Hash(string password)
        => BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);

    public bool Verify(string password, string passwordHash)
        => BCrypt.Net.BCrypt.Verify(password, passwordHash);
}
