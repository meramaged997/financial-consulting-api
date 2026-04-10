using Startawy.API.Extensions;
using Startawy.API.Middleware;
using Startawy.Application.Common;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Startawy.Infrastructure.Data;
using Startawy.Infrastructure.Data.Seed;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Ensure Kestrel binds the HTTPS development port (7289) using default dev cert.
builder.WebHost.ConfigureKestrel(options =>
{
    // Listen on localhost:7289 with HTTPS
    options.ListenLocalhost(7289, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

// ── Register Services ────────────────────────────────────────────────────
builder.Services.AddMemoryCache();
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        // Allow enums to be sent/received as strings (e.g. "Revenue") for client friendliness.
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? "Invalid value." : e.ErrorMessage)
                .ToList();

            return new BadRequestObjectResult(ApiResponse<object>.Fail("Validation failed.", errors));
        };
    });
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddSwaggerWithJwt();

builder.Services.AddHttpLogging(o =>
{
    o.LoggingFields = HttpLoggingFields.RequestMethod |
                      HttpLoggingFields.RequestPath |
                      HttpLoggingFields.ResponseStatusCode |
                      HttpLoggingFields.Duration;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendLocalhost", policy =>
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// ── Build App ───────────────────────────────────────────────────────────
var app = builder.Build();

// ── Middleware Pipeline ──────────────────────────────────────────────────
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpLogging();

// Apply migrations & seed (safe to run on startup).
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (db.Database.IsRelational())
    {
        // Safety: if the DB already has tables but no EF migrations history,
        // applying InitialCreate will fail (and can partially mutate schema).
        // In that case, stop with a clear message so the operator can point to a clean DB
        // or baseline the existing schema intentionally.
        var conn = db.Database.GetDbConnection();
        await conn.OpenAsync();
        await using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = @"
SELECT
  CASE WHEN OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL THEN 0 ELSE 1 END AS HasHistory,
  CASE WHEN OBJECT_ID(N'[cash_flow_forecasts]') IS NULL THEN 0 ELSE 1 END AS HasCashFlowForecasts;";
            await using var reader = await cmd.ExecuteReaderAsync();
            await reader.ReadAsync();
            var hasHistory = reader.GetInt32(0) == 1;
            var hasAnyKnownTable = reader.GetInt32(1) == 1;

            if (!hasHistory && hasAnyKnownTable)
            {
                throw new InvalidOperationException(
                    "Database already contains Startawy tables but is not tracked by EF migrations (missing __EFMigrationsHistory). " +
                    "Point the API to a clean/empty database, or baseline the existing schema before applying migrations.");
            }
        }

        await db.Database.MigrateAsync();
    }
    else
        await db.Database.EnsureCreatedAsync();
    await SeedData.SeedAsync(db);
}



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Startawy API v1");
        c.RoutePrefix = string.Empty;   // Swagger at root "/"
    });
}

app.UseHttpsRedirection();                        // 2. HTTPS redirect
app.UseCors("FrontendLocalhost");                 // 3. CORS
app.UseAuthentication();                          // 4. Validate JWT
app.UseAuthorization();                           // 5. Check permissions
app.MapControllers();                             // 6. Route to controllers

app.Run();

public partial class Program { }
