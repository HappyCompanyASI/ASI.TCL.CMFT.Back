using ASI.TCL.CMFT.Domain.SYS;
using ASI.TCL.CMFT.Infrastructure.EFCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASI.TCL.CMFT.Infrastructure.EFCore.EntityTypeConfiguration
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        { 
            builder.ToTable("user", "dbo");
            // 主鍵：Id
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .HasColumnOrder(0)
                .ValueGeneratedOnAdd();

            // UserName -> Account 欄位
            builder.Property(u => u.UserName)
                .HasColumnName("Account")
                .HasColumnOrder(1)
                .IsRequired()
                .HasMaxLength(40);
            builder.HasIndex(u => u.UserName)
                .IsUnique();

            // FirstName：nvarchar(40)，必填
            builder.Property(u => u.FirstName)
                .HasColumnOrder(2)
                .IsRequired()
                .HasMaxLength(40);

            // LastName：nvarchar(40)，必填
            builder.Property(u => u.LastName)
                .HasColumnOrder(3)
                .IsRequired()
                .HasMaxLength(40);

            // PasswordHash：nvarchar(max)，必填
            builder.Property(u => u.PasswordHash)
                .HasColumnOrder(4)
                .IsRequired()
                .HasColumnType("text");

            // BelongUnit: 所屬單位，必填、長度 40
            builder.Property(r => r.BelongUnit)
                .HasColumnOrder(5)
                .IsRequired()
                .HasMaxLength(40);

            // IsActive：bit，預設 1（啟用）
            builder.Property(u => u.IsActive)
                .HasColumnOrder(6)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }

}