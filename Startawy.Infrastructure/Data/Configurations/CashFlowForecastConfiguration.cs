using startawy.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace startawy.Infrastructure.Data.Configurations;

public class CashFlowForecastConfiguration : IEntityTypeConfiguration<CashFlowForecast>
{
    public void Configure(EntityTypeBuilder<CashFlowForecast> b)
    {
        b.ToTable("cash_flow_forecasts");
        b.HasKey(e => e.Id);
        b.Property(e => e.InitialCashBalance).HasPrecision(18, 2);
        b.Property(e => e.MonthlyRevenueTrend).HasPrecision(18, 2);
        b.Property(e => e.MonthlyExpenseTrend).HasPrecision(18, 2);
        b.Property(e => e.GrowthRate).HasPrecision(18, 6);
        b.Property(e => e.ProjectedRunway).HasPrecision(18, 2);
        b.HasMany(e => e.MonthlyForecasts).WithOne().HasForeignKey(m => m.CashFlowForecastId).OnDelete(DeleteBehavior.Cascade);
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}
