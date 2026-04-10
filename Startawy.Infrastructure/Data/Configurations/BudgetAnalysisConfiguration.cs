using startawy.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace startawy.Infrastructure.Data.Configurations;

public class BudgetAnalysisConfiguration : IEntityTypeConfiguration<BudgetAnalysis>
{
    public void Configure(EntityTypeBuilder<BudgetAnalysis> b)
    {
        b.ToTable("budget_analyses");
        b.HasKey(e => e.Id);
        b.Property(e => e.TotalRevenue).HasPrecision(18, 2);
        b.Property(e => e.FixedCosts).HasPrecision(18, 2);
        b.Property(e => e.VariableCosts).HasPrecision(18, 2);
        b.Property(e => e.TotalExpenses).HasPrecision(18, 2);
        b.Property(e => e.ProfitStatus).HasMaxLength(50);
        b.Property(e => e.RiskLevel).HasConversion<string>();
        b.Ignore(e => e.NetProfit);
        b.Ignore(e => e.ProfitMargin);
        b.HasMany(e => e.LineItems)
            .WithOne(li => li.BudgetAnalysis)
            .HasForeignKey(li => li.BudgetAnalysisId)
            .OnDelete(DeleteBehavior.Cascade);
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}
