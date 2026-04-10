using startawy.Core.Enums;

namespace startawy.Core.Interfaces.Services;

public record UserDto(
    string      Id,
    string      Email,
    string      FirstName,
    string      LastName,
    string      CompanyName,
    string      Industry,
    PackageType Package,
    DateTime    CreatedAt,
    bool        IsActive
);

public interface IUserService
{
    Task<UserDto?>            GetByIdAsync(string userId, CancellationToken ct = default);
    Task<UserDto?>            GetByEmailAsync(string email, CancellationToken ct = default);
    Task<(bool success, string? error)> CreateAsync(string email, string password, string firstName, string lastName, string company, string industry, CancellationToken ct = default);
    Task<(bool success, string? error)> CheckPasswordAsync(string userId, string password, CancellationToken ct = default);
    Task<(bool success, string? error)> ChangePasswordAsync(string userId, string currentPassword, string newPassword, CancellationToken ct = default);
    Task<UserDto?>            UpdateAsync(string userId, string? firstName, string? lastName, string? company, string? industry, CancellationToken ct = default);
    Task                      UpdatePackageAsync(string userId, PackageType package, CancellationToken ct = default);
    Task<string>              GetRoleAsync(string userId, CancellationToken ct = default);
}
