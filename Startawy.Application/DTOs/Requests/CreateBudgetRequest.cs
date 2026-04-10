using startawy.Core.Enums;

namespace Startawy.Application.DTOs.Requests;

public record CreateBudgetRequest(
    string BusinessName,
    string Industry,
    string Period,
    List<BudgetLineItemRequest> LineItems
);

public record BudgetLineItemRequest(
    string      Category,
    string      Description,
    decimal     PlannedAmount,
    decimal     ActualAmount,
    LineItemType Type
);
