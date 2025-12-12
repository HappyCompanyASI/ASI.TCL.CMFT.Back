using ASI.TCL.CMFT.Infrastructure.EFCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASI.TCL.CMFT.Infrastructure.EFCore.EntityTypeConfiguration
{
    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.ToTable("role", "dbo");

            // 主鍵 Id
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id)
                .HasColumnOrder(0)
                .ValueGeneratedOnAdd();

            // Name: 角色名稱，必填、長度 40，且唯一
            builder.Property(r => r.Name)
                .HasColumnOrder(1)
                .IsRequired()
                .HasMaxLength(40);

            builder.HasIndex(r => r.Name)
                .IsUnique();

            // Description: nvarchar(max)，可為空
            builder.Property(r => r.Description)
                .HasColumnOrder(3)
                .HasColumnType("text");

            // IsActive: bit，必填，預設 true (1)
            builder.Property(r => r.IsActive)
                .HasColumnOrder(4)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}
