using startawy.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace startawy.Infrastructure.Data.Configurations;

public class MarketTrendConfiguration : IEntityTypeConfiguration<MarketTrend>
{
    public void Configure(EntityTypeBuilder<MarketTrend> b)
    {
        b.ToTable("market_trends");
        b.HasKey(e => e.Id);
        b.Property(e => e.Direction).HasConversion<string>();
    }
}
