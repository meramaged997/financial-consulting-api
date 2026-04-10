using startawy.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace startawy.Infrastructure.Data.Configurations;

public class DashboardSnapshotConfiguration : IEntityTypeConfiguration<DashboardSnapshot>
{
    public void Configure(EntityTypeBuilder<DashboardSnapshot> b)
    {
        b.ToTable("dashboard_snapshots");
        b.HasKey(e => e.Id);
        b.Property(e => e.Revenue).HasPrecision(18, 2);
        b.Property(e => e.Expenses).HasPrecision(18, 2);
        b.Property(e => e.NetProfit).HasPrecision(18, 2);
        b.Property(e => e.CashBalance).HasPrecision(18, 2);
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}
