using Microsoft.EntityFrameworkCore;

namespace ASI.TCL.CMFT.Infrastructure.EFCore.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void UseSnakeCaseNames(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // 表名轉換
                var tableName = entity.GetTableName();
                entity.SetTableName(ToSnakeCase(tableName));

                // 欄位轉換
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(ToSnakeCase(property.Name));
                }

                // 主鍵名稱轉換
                foreach (var key in entity.GetKeys())
                {
                    key.SetName(ToSnakeCase(key.GetName()));
                }

                // 索引轉換
                foreach (var index in entity.GetIndexes())
                {
                    index.SetDatabaseName(ToSnakeCase(index.GetDatabaseName()));
                }

                // 外鍵 constraint 名稱轉換
                foreach (var fk in entity.GetForeignKeys())
                {
                    fk.SetConstraintName(ToSnakeCase(fk.GetConstraintName()));
                }
            }
        }

        private static string ToSnakeCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            var builder = new System.Text.StringBuilder();
            builder.Append(char.ToLowerInvariant(input[0]));

            for (int i = 1; i < input.Length; i++)
            {
                var c = input[i];
                if (char.IsUpper(c))
                {
                    builder.Append('_');
                    builder.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    builder.Append(c);
                }
            }

            return builder.ToString();
        }
    }
}