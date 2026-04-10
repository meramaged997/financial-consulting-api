using startawy.Core.Enums;

namespace startawy.Core.Entities;

public class MarketingCampaign : AuditableEntity
{
    public string         UserId                 { get; set; } = string.Empty;
    public string         CampaignName           { get; set; } = string.Empty;
    public string         BusinessType           { get; set; } = string.Empty;
    public string         TargetAudience         { get; set; } = string.Empty;
    public decimal        Budget                 { get; set; }
    public string         Strategy               { get; set; } = string.Empty;
    public string         ChannelRecommendations { get; set; } = string.Empty;
    public string         ContentCalendar        { get; set; } = string.Empty;
    public CampaignStatus Status                 { get; set; } = CampaignStatus.Draft;
    public DateTime       StartDate              { get; set; }
    public DateTime       EndDate                { get; set; }
}
