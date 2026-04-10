using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace startawy.API.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration config)
    {
        var settings = config.GetSection("JwtSettings");
        var key      = Encoding.UTF8.GetBytes(settings["SecretKey"]!);

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey        = new SymmetricSecurityKey(key),
                ValidateIssuer          = true,
                ValidIssuer             = settings["Issuer"],
                ValidateAudience        = true,
                ValidAudience           = settings["Audience"],
                ValidateLifetime        = true
            };
        });

        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("PremiumOnly",  p => p.RequireAssertion(ctx => ctx.User.HasClaim("Package", "Premium")));
            opt.AddPolicy("BasicOrAbove", p => p.RequireAssertion(ctx =>
                ctx.User.HasClaim("Package", "Basic") || ctx.User.HasClaim("Package", "Premium")));
        });

        return services;
    }
}
