using startawy.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace startawy.Infrastructure.Data.Configurations;

public class MarketResearchConfiguration : IEntityTypeConfiguration<MarketResearch>
{
    public void Configure(EntityTypeBuilder<MarketResearch> b)
    {
        b.ToTable("market_researches");
        b.HasKey(e => e.Id);
        b.Property(e => e.EstimatedMarketSize).HasPrecision(18, 2);
        b.HasMany(e => e.Competitors).WithOne().HasForeignKey(c => c.MarketResearchId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.Trends).WithOne().HasForeignKey(t => t.MarketResearchId).OnDelete(DeleteBehavior.Cascade);
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}
