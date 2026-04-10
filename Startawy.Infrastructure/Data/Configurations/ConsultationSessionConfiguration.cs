using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using startawy.Core.Entities;

namespace startawy.Infrastructure.Data.Configurations;

public class ConsultationSessionConfiguration : IEntityTypeConfiguration<ConsultationSession>
{
    public void Configure(EntityTypeBuilder<ConsultationSession> b)
    {
        b.ToTable("consultation_sessions");
        b.HasKey(x => x.Id);
        b.Property(x => x.Fee).HasPrecision(18, 2);
        b.Property(x => x.Status).HasMaxLength(30);
        b.HasIndex(x => new { x.ConsultantUserId, x.StartAtUtc, x.EndAtUtc });
        b.HasIndex(x => new { x.FounderUserId, x.StartAtUtc, x.EndAtUtc });

        b.HasOne(x => x.FounderUser)
            .WithMany()
            .HasForeignKey(x => x.FounderUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.ConsultantUser)
            .WithMany()
            .HasForeignKey(x => x.ConsultantUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasQueryFilter(x => !x.IsDeleted);
    }
}

