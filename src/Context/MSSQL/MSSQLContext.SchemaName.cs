using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;

namespace MtdKey.Storage.Context.MSSQL
{
    internal partial class MSSQLContext : DbContext
    {
        internal virtual DbSet<SchemaName> SchemaNames { get; set; }

        private static void SchemaNameModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SchemaName>(entity =>
            {
                entity.ToTable("schema_name");                

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint");

                entity.HasIndex(e => e.UniqueName)
                    .IsUnique()
                    .HasDatabaseName("idx_schema_unique_name");

                entity.Property(e => e.UniqueName)
                    .IsRequired()
                    .HasColumnName("unique_name")
                    .HasColumnType("varchar(256)");                
            });

        }
    }
}
