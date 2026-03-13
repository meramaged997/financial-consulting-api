using Startawy.API.Extensions;
using Startawy.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ── Register Services ────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddSwaggerWithJwt();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// ── Build App ────────────────────────────────────────────────────────────
var app = builder.Build();

// ── Middleware Pipeline ──────────────────────────────────────────────────
// app.UseMiddleware<GlobalExceptionMiddleware>();



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
app.UseCors("AllowAll");                          // 3. CORS
app.UseAuthentication();                          // 4. Validate JWT
app.UseAuthorization();                           // 5. Check permissions
app.MapControllers();                             // 6. Route to controllers

app.Run();
