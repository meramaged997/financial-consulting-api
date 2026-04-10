using startawy.Core.Enums;

namespace startawy.Application.DTOs.Responses;

public record MarketingCampaignResponse(
    int           Id,
    string        CampaignName,
    string        BusinessType,
    string        TargetAudience,
    decimal       Budget,
    string        Strategy,
    string        ChannelRecommendations,
    string        ContentCalendar,
    CampaignStatus Status,
    DateTime      StartDate,
    DateTime      EndDate,
    DateTime      CreatedAt
);
