using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using startawy.Core.Entities;

namespace startawy.Infrastructure.Data.Configurations;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> b)
    {
        b.ToTable("chat_messages");
        b.HasKey(x => x.Id);
        b.Property(x => x.Role).HasMaxLength(30);
        b.HasIndex(x => new { x.ChatSessionId, x.SentAt });
        b.HasQueryFilter(x => !x.IsDeleted);
    }
}

