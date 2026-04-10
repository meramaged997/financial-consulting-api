using Startawy.Domain.Entities;

namespace Startawy.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user, string packageType);
    DateTime GetTokenExpiry();
}
