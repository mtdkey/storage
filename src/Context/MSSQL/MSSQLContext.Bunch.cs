using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage.Context.MSSQL
{
    internal partial class MSSQLContext : DbContext
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

                entity.Property(e => e.SchemaId)
                    .IsRequired()
                    .HasColumnName("schema_id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.ArchiveFlag)
                    .IsRequired()
                    .HasColumnName("archive_flag")
                    .HasColumnType("tinyint");

                entity.Property(e => e.DeletedFlag)
                    .IsRequired()
                    .HasColumnName("deleted_flag")
                    .HasColumnType("tinyint");
            });

            modelBuilder.Entity<Bunch>().HasQueryFilter(p => p.DeletedFlag == 0);

        }
    }
}
