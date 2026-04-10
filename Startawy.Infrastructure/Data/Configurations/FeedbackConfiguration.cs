using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using startawy.Core.Entities;

namespace startawy.Infrastructure.Data.Configurations;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> b)
    {
        b.ToTable("feedback");
        b.HasKey(x => x.Id);
        b.Property(x => x.Category).HasMaxLength(30);
        b.HasIndex(x => new { x.IsReviewed, x.Category });

        b.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasQueryFilter(x => !x.IsDeleted);
    }
}

