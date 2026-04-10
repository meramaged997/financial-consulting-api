using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using startawy.Core.Entities;

namespace startawy.Infrastructure.Data.Configurations;

public class ConsultantAvailabilitySlotConfiguration : IEntityTypeConfiguration<ConsultantAvailabilitySlot>
{
    public void Configure(EntityTypeBuilder<ConsultantAvailabilitySlot> b)
    {
        b.ToTable("consultant_availability_slots");
        b.HasKey(x => x.Id);
        b.HasIndex(x => new { x.ConsultantUserId, x.StartAtUtc, x.EndAtUtc }).IsUnique();
        b.Property(x => x.ConsultantUserId).IsRequired();

        b.HasOne(x => x.ConsultantUser)
            .WithMany()
            .HasForeignKey(x => x.ConsultantUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasQueryFilter(x => !x.IsDeleted);
    }
}

