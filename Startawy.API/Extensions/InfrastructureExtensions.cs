using Microsoft.EntityFrameworkCore;
using Startawy.Infrastructure.Data;

namespace Startawy.API.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        return services;
    }
}
