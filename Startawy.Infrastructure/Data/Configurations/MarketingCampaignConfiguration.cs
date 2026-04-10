using startawy.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace startawy.Infrastructure.Data.Configurations;

public class MarketingCampaignConfiguration : IEntityTypeConfiguration<MarketingCampaign>
{
    public void Configure(EntityTypeBuilder<MarketingCampaign> b)
    {
        b.ToTable("marketing_campaigns");
        b.HasKey(e => e.Id);
        b.Property(e => e.Budget).HasPrecision(18, 2);
        b.Property(e => e.Status).HasConversion<string>();
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}
