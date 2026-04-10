using System;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using startawy.Core.Interfaces.Repositories;
using Startawy.Application.Interfaces;
using Startawy.Application.Services;
using Startawy.Application.Common;
using Startawy.Domain.Interfaces;
using Startawy.Infrastructure.Data;
using Startawy.Infrastructure.Repositories;
using startawy.Infrastructure.Repositories;
using Startawy.Infrastructure.Services;
using startawy.Infrastructure.Services;

namespace Startawy.API.Extensions;

public static class ServiceExtensions
{
    // ── Database ──────────────────────────────────────────────────────────
    public static IServiceCollection AddDatabase(
        this IServiceCollection services, IConfiguration config)
    {
        var conn = config.GetConnectionString("DefaultConnection")
                   ?? throw new InvalidOperationException("DefaultConnection missing.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(conn, b => b.MigrationsAssembly("Startawy.Infrastructure")));

        return services;
    }

    // ── JWT Authentication ────────────────────────────────────────────────
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services, IConfiguration config)
    {
        var secretKey = config["Jwt:SecretKey"]
            ?? throw new InvalidOperationException("Jwt:SecretKey is missing.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Relax issuer/audience validation to avoid 401s caused by minor mismatches
                    ValidateIssuer           = false,
                    ValidateAudience         = false,
                    ValidateLifetime         = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer              = config["Jwt:Issuer"],
                    ValidAudience            = config["Jwt:Audience"],
                    IssuerSigningKey         = key,
                    ClockSkew                = TimeSpan.Zero
                };

                // Ensure auth failures use the same JSON envelope.
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        // Skip the default behavior (which writes a plain-text response).
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        var payload = JsonSerializer.Serialize(
                            ApiResponse<object>.Fail("Unauthorized. Missing or invalid JWT token."),
                            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                        await context.Response.WriteAsync(payload);
                    },
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";

                        var payload = JsonSerializer.Serialize(
                            ApiResponse<object>.Fail("Forbidden. You don't have permission to access this resource."),
                            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                        await context.Response.WriteAsync(payload);
                    }
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("BasicOrAbove", policy =>
                policy.RequireAssertion(ctx =>
                {
                    var pkg = ctx.User.FindFirst("package")?.Value;
                    return !string.IsNullOrEmpty(pkg) &&
                           (pkg.Equals("Basic", StringComparison.OrdinalIgnoreCase) ||
                            pkg.Equals("Premium", StringComparison.OrdinalIgnoreCase));
                }));

            options.AddPolicy("PremiumOnly", policy =>
                policy.RequireAssertion(ctx =>
                {
                    var pkg = ctx.User.FindFirst("package")?.Value;
                    return pkg != null &&
                           pkg.Equals("Premium", StringComparison.OrdinalIgnoreCase);
                }));
        });

        return services;
    }

    // ── Application & Domain Services ────────────────────────────────────
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        // Domain/Infrastructure repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPackageRepository, PackageRepository>();
        services.AddScoped<IBudgetRepository, BudgetRepository>();
        services.AddScoped<IConsultationRepository, ConsultationRepository>();
        services.AddScoped<IDashboardRepository, DashboardRepository>();
        services.AddScoped<ICashFlowRepository, CashFlowRepository>();
        services.AddScoped<IFinancialRepository, FinancialRepository>();
        services.AddScoped<IMarketResearchRepository, MarketResearchRepository>();
        services.AddScoped<IMarketingRepository, MarketingRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IConsultantAvailabilityRepository, ConsultantAvailabilityRepository>();
        services.AddScoped<IConsultationSessionRepository, ConsultationSessionRepository>();
        services.AddScoped<IFeedbackRepository, FeedbackRepository>();
        services.AddScoped<IFollowUpPlanRepository, FollowUpPlanRepository>();
        services.AddScoped<IConsultantRepository, ConsultantRepository>();

        // Application services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPackageService, PackageService>();
        services.AddScoped<IBudgetService, BudgetService>();
        services.AddScoped<IConsultationService, ConsultationService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<ICashFlowService, CashFlowService>();
        services.AddScoped<IFinancialService, FinancialService>();
        services.AddScoped<IMarketResearchService, MarketResearchService>();
        services.AddScoped<IMarketingService, MarketingService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<ISessionsService, SessionsService>();
        services.AddScoped<IFeedbackService, FeedbackService>();
        services.AddScoped<IFollowUpPlanService, FollowUpPlanService>();
        services.AddScoped<IPaymentService, PaymentService>();

        // Infrastructure services
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
        services.AddHttpClient<startawy.Core.Interfaces.Services.IAIService, OpenAIService>();

        return services;
    }

    // ── Swagger with JWT support ──────────────────────────────────────────
    public static IServiceCollection AddSwaggerWithJwt(
        this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title       = "Startawy API",
                Version     = "v1",
                Description = "Startawy Platform – REST API"
            });

            var jwtScheme = new OpenApiSecurityScheme
            {
                Name         = "Authorization",
                Description  = "Enter: Bearer {your_token}",
                In           = ParameterLocation.Header,
                Type         = SecuritySchemeType.Http,
                Scheme       = "bearer",
                BearerFormat = "JWT",
                Reference    = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = JwtBearerDefaults.AuthenticationScheme
                }
            };

            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, jwtScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtScheme, Array.Empty<string>() }
            });
        });

        return services;
    }
}

