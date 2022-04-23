using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;

namespace MtdKey.Storage.Context.MSSQL
{
    internal partial class MSSQLContext : DbContext
    {
        internal virtual DbSet<SchemaVersion> SchemaVersions { get; set; }

        private static void SchemaVersionModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SchemaVersion>(entity =>
            {
                entity.ToTable("schema_version");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint");

                entity.HasIndex(e => e.SchemaNameId)
                    .HasDatabaseName("fk_schema_version_idx");

                entity.Property(e => e.SchemaNameId)
                    .HasColumnName("schema_name_id")
                    .HasColumnType("bigint");

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasColumnName("version")
                    .HasColumnType("bigint");

                entity.Property(e => e.XmlSchema)
                    .IsRequired()
                    .HasColumnName("xml_schema")
                    .HasColumnType("text");


                entity.HasOne(d => d.SchemaName)
                    .WithMany(p => p.SchemaVersions)
                    .HasForeignKey(d => d.SchemaNameId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_schema_version");
            });

        }
    }
}
