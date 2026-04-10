using startawy.Core.Enums;

namespace startawy.Core.Interfaces.Services;

public interface ICurrentUserService
{
    string      UserId          { get; }
    string      Email           { get; }
    PackageType Package         { get; }
    string      Role            { get; }
    bool        IsAuthenticated { get; }
}
