using ASI.TCL.CMFT.Domain.DMD;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASI.TCL.CMFT.Infrastructure.EFCore.EntityTypeConfiguration
{
    public class MessageConfiguration : AuditableEntityTypeConfiguration<Message>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("message", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasConversion(id => id.Value, value => new MessageId(value))
                .IsRequired();

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Content)
                .IsRequired();

            builder.Property(x => x.BelongGroupId)
                .HasConversion(
                    id => id != null ? id.Value : (Guid?)null,
                    value => value.HasValue ? new MessageGroupId(value.Value) : null
                )
                .IsRequired(false);

            builder.Property(x => x.IsUseDu)
                .IsRequired();

            builder
                // 每一筆 Message 都有一個 Group 導航屬性
                .HasOne(x => x.Group)
                // 對應的 Group 物件擁有 Messages 集合
                .WithMany(g => g.Messages)
                // Message 的 BelongGroupId 是 FK（外鍵）
                .HasForeignKey(x => x.BelongGroupId);
        }
    }
}