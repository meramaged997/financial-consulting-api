using startawy.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace startawy.Infrastructure.Data.Configurations;

public class BudgetLineItemConfiguration : IEntityTypeConfiguration<BudgetLineItem>
{
    public void Configure(EntityTypeBuilder<BudgetLineItem> b)
    {
        b.ToTable("budget_line_items");
        b.HasKey(e => e.Id);
        b.Property(e => e.PlannedAmount).HasPrecision(18, 2);
        b.Property(e => e.ActualAmount).HasPrecision(18, 2);
        b.Property(e => e.Type).HasConversion<string>();
        b.Ignore(e => e.Variance);
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}
