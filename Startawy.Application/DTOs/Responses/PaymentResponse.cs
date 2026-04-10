namespace Startawy.Application.DTOs.Responses;

public record PaymentResponse(
    string TransactionId,
    decimal Amount,
    string Type,
    string PaymentMethod,
    DateOnly TransactionDate,
    string SubscriptionId,
    string PackageType
);

