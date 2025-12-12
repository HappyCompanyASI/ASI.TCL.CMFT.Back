using ASI.TCL.CMFT.Domain.DMD;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASI.TCL.CMFT.Infrastructure.EFCore.EntityTypeConfiguration
{
    public class MessageGroupConfiguration : IEntityTypeConfiguration<MessageGroup>
    {
        public void Configure(EntityTypeBuilder<MessageGroup> builder)
        {
            builder.ToTable("message_group", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasConversion(id => id.Value, value => new MessageGroupId(value))
                .IsRequired();

            builder.Property(x => x.GroupName)
                .HasMaxLength(200)
                .IsRequired();

            builder
                //每個 Group 擁有多筆 Message
                .HasMany(x => x.Messages)
                //每筆 Message 對應一個 Group
                .WithOne(m => m.Group)
                //同樣指定 BelongGroupId 為外鍵
                .HasForeignKey(m => m.BelongGroupId)
                // Group 被刪除時，把 Message 的 FK 設為 null（不 cascade 刪除）
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}