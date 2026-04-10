using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using startawy.Core.Entities;

namespace startawy.Infrastructure.Data.Configurations;

public class FollowUpPlanConfiguration : IEntityTypeConfiguration<FollowUpPlan>
{
    public void Configure(EntityTypeBuilder<FollowUpPlan> b)
    {
        b.ToTable("follow_up_plans");
        b.HasKey(x => x.Id);
        b.Property(x => x.Goal).HasMaxLength(500);

        b.HasOne(x => x.FounderUser)
            .WithMany()
            .HasForeignKey(x => x.FounderUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.ConsultantUser)
            .WithMany()
            .HasForeignKey(x => x.ConsultantUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasMany(x => x.Steps)
            .WithOne(s => s.FollowUpPlan)
            .HasForeignKey(s => s.FollowUpPlanId)
            .OnDelete(DeleteBehavior.Cascade);
        b.HasQueryFilter(x => !x.IsDeleted);
    }
}

