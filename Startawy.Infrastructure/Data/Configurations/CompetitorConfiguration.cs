using startawy.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace startawy.Infrastructure.Data.Configurations;

public class CompetitorConfiguration : IEntityTypeConfiguration<Competitor>
{
    public void Configure(EntityTypeBuilder<Competitor> b)
    {
        b.ToTable("competitors");
        b.HasKey(e => e.Id);
    }
}
