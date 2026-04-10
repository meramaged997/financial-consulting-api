using startawy.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace startawy.Infrastructure.Data.Configurations;

public class ConsultationRequestConfiguration : IEntityTypeConfiguration<ConsultationRequest>
{
    public void Configure(EntityTypeBuilder<ConsultationRequest> b)
    {
        b.ToTable("consultation_requests");
        b.HasKey(e => e.Id);
        b.Property(e => e.Type).HasConversion<string>();
        b.Property(e => e.Status).HasConversion<string>();
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}
