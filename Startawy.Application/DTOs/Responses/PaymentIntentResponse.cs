namespace Startawy.Application.DTOs.Responses;

public record PaymentIntentResponse(
    string TransactionId,
    string Status,
    decimal Amount,
    string Currency,
    string Purpose
);

