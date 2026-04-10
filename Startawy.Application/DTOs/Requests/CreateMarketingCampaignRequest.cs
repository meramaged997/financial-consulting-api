namespace Startawy.Application.DTOs.Requests;

public record CreateMarketingCampaignRequest(
    string   CampaignName,
    string   BusinessType,
    string   TargetAudience,
    decimal  Budget,
    string?  ProductDescription,
    string?  UniqueValueProposition,
    DateTime StartDate,
    DateTime EndDate
);
