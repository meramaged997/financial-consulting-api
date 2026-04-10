using startawy.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace startawy.Infrastructure.Data.Configurations;

public class ChatSessionConfiguration : IEntityTypeConfiguration<ChatSession>
{
    public void Configure(EntityTypeBuilder<ChatSession> b)
    {
        b.ToTable("chat_sessions");
        b.HasKey(e => e.Id);
        b.HasMany(e => e.Messages)
            .WithOne(m => m.ChatSession)
            .HasForeignKey(m => m.ChatSessionId)
            .OnDelete(DeleteBehavior.Cascade);
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}
