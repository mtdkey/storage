using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;
using System;

namespace MtdKey.Storage.Context.MySQL
{
    internal partial class MySQLContext : DbContext
    {
        internal virtual DbSet<Bunch> Bunches { get; set; }

        private static void BunchModelCreating(ModelBuilder modelBuilder)
        {
  
            modelBuilder.Entity<Bunch>(entity =>
            {
                entity.ToTable("bunch");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("nvarchar(128)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("nvarchar(256)");

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasColumnName("version")
                    .HasColumnType("float");

                entity.Property(e => e.SchemaId)
                    .IsRequired()
                    .HasColumnName("schema_id")
                    .HasColumnType("char(36)");                    

                entity.Property(e => e.ArchiveFlag)
                    .IsRequired()
                    .HasColumnName("archive_flag")
                    .HasColumnType("tinyint(2)");                    

                entity.Property(e => e.DeletedFlag)
                    .IsRequired()
                    .HasColumnName("deleted_flag")
                    .HasColumnType("tinyint(2)");
            });
            modelBuilder.Entity<Bunch>().HasQueryFilter(p => p.DeletedFlag == 0);
        }
    }
}
