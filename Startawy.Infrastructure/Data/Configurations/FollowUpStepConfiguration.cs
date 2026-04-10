using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using startawy.Core.Entities;

namespace startawy.Infrastructure.Data.Configurations;

public class FollowUpStepConfiguration : IEntityTypeConfiguration<FollowUpStep>
{
    public void Configure(EntityTypeBuilder<FollowUpStep> b)
    {
        b.ToTable("follow_up_steps");
        b.HasKey(x => x.Id);
        b.Property(x => x.Status).HasMaxLength(30);
        b.HasIndex(x => new { x.FollowUpPlanId, x.DueAtUtc });
        b.HasQueryFilter(x => !x.IsDeleted);
    }
}

