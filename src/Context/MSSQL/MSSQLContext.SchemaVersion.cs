using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;
using System;

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

                entity.Property(e => e.XMLData)
                    .IsRequired()
                    .HasColumnName("xml_data")
                    .HasColumnType("text");
            });
        }
    }
}
