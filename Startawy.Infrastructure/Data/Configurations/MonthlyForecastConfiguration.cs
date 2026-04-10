using startawy.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace startawy.Infrastructure.Data.Configurations;

public class MonthlyForecastConfiguration : IEntityTypeConfiguration<MonthlyForecast>
{
    public void Configure(EntityTypeBuilder<MonthlyForecast> b)
    {
        b.ToTable("monthly_forecasts");
        b.HasKey(e => e.Id);
        b.Property(e => e.ProjectedRevenue).HasPrecision(18, 2);
        b.Property(e => e.ProjectedExpenses).HasPrecision(18, 2);
        b.Property(e => e.CumulativeCashBalance).HasPrecision(18, 2);
        b.Ignore(e => e.ProjectedNetCashFlow);
    }
}
