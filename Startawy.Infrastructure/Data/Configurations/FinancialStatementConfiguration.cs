using startawy.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace startawy.Infrastructure.Data.Configurations;

public class FinancialStatementConfiguration : IEntityTypeConfiguration<FinancialStatement>
{
    public void Configure(EntityTypeBuilder<FinancialStatement> b)
    {
        b.ToTable("financial_statements");
        b.HasKey(e => e.Id);
        b.Property(e => e.GrossRevenue).HasPrecision(18, 2);
        b.Property(e => e.CostOfGoodsSold).HasPrecision(18, 2);
        b.Property(e => e.OperatingExpenses).HasPrecision(18, 2);
        b.Property(e => e.NetIncome).HasPrecision(18, 2);
        b.Property(e => e.TotalAssets).HasPrecision(18, 2);
        b.Property(e => e.TotalLiabilities).HasPrecision(18, 2);
        b.Property(e => e.OperatingCashFlow).HasPrecision(18, 2);
        b.Property(e => e.InvestingCashFlow).HasPrecision(18, 2);
        b.Property(e => e.FinancingCashFlow).HasPrecision(18, 2);
        b.Property(e => e.Type).HasConversion<string>();
        b.Property(e => e.RiskAssessment).HasConversion<string>();
        b.Ignore(e => e.GrossProfit);
        b.Ignore(e => e.OperatingIncome);
        b.Ignore(e => e.Equity);
        b.Ignore(e => e.NetCashFlow);
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}
