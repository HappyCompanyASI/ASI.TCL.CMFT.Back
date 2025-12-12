using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASI.TCL.CMFT.Infrastructure.EFCore.EntityTypeConfiguration
{
    public abstract class AuditableEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            ConfigureEntity(builder); // 讓子類去決定欄位順序
            ConfigureAuditFields(builder); // 自動加上 CreatedBy 等欄位
        }

        // 子類只需實作這個，不需再呼叫 ConfigureAuditFields
        protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);

        private void ConfigureAuditFields(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property("CreatedBy").HasMaxLength(100).IsRequired();
            builder.Property("UpdatedBy").HasMaxLength(100).IsRequired();
            builder.Property("CreatedAt").IsRequired();
            builder.Property("UpdatedAt").IsRequired();
        }
    }
}