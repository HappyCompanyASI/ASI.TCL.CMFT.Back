using ASI.TCL.CMFT.Domain.PA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASI.TCL.CMFT.Infrastructure.EFCore.EntityTypeConfiguration
{
    public class VoiceGroupConfiguration : IEntityTypeConfiguration<VoiceGroup>
    {
        public void Configure(EntityTypeBuilder<VoiceGroup> builder)
        {
            builder.ToTable("voice_group", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasConversion(id => id.Value, value => new VoiceGroupId(value))
                .IsRequired();

            builder.Property(x => x.GroupName)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .HasMany(x => x.Voices)
                .WithOne() // 沒有 navigation property
                .HasForeignKey(m => m.BelongGroupId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}