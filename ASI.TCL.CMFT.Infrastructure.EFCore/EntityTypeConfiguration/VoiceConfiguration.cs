using ASI.TCL.CMFT.Domain.PA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASI.TCL.CMFT.Infrastructure.EFCore.EntityTypeConfiguration
{
    public class VoiceConfiguration : IEntityTypeConfiguration<Voice>
    {
        public void Configure(EntityTypeBuilder<Voice> builder)
        {
            builder.ToTable("voice", "dbo");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Id)
                .HasConversion(id => id.Value, value => new VoiceId(value))
                .IsRequired();

            builder.Property(v => v.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(v => v.Content)
                .IsRequired();

            builder.Property(v => v.IsChn).IsRequired();
            builder.Property(v => v.IsTwn).IsRequired();
            builder.Property(v => v.IsHakka).IsRequired();
            builder.Property(v => v.IsEng).IsRequired();

            builder.Property(v => v.BelongGroupId)
                .HasConversion(
                    id => id != null ? id.Value : (Guid?)null,
                    value => value.HasValue ? new VoiceGroupId(value.Value) : null
                )
                .IsRequired(false);
        }
    }
}