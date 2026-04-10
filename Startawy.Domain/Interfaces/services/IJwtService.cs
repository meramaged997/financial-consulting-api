using startawy.Core.Enums;

namespace startawy.Core.Interfaces.Services;

public interface IJwtService
{
    string  GenerateToken(string userId, string email, string fullName, string role, PackageType package);
    string? ValidateToken(string token);
}
