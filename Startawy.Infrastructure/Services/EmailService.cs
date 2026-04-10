using startawy.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace startawy.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger) => _logger = logger;

    public Task SendWelcomeAsync(string email, string firstName, CancellationToken ct = default)
    {
        _logger.LogInformation("Welcome email → {Email} ({Name})", email, firstName);
        // TODO: Integrate SendGrid or SMTP
        return Task.CompletedTask;
    }

    public Task SendConsultationConfirmationAsync(string email, string subject, CancellationToken ct = default)
    {
        _logger.LogInformation("Consultation confirmation → {Email} | {Subject}", email, subject);
        return Task.CompletedTask;
    }

    public Task SendPackageUpgradeAsync(string email, string packageName, CancellationToken ct = default)
    {
        _logger.LogInformation("Package upgrade → {Email} | {Package}", email, packageName);
        return Task.CompletedTask;
    }
}
