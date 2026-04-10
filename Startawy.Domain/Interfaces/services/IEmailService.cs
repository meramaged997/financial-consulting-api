namespace startawy.Core.Interfaces.Services;

public interface IEmailService
{
    Task SendWelcomeAsync(string email, string firstName, CancellationToken ct = default);
    Task SendConsultationConfirmationAsync(string email, string subject, CancellationToken ct = default);
    Task SendPackageUpgradeAsync(string email, string packageName, CancellationToken ct = default);
}
