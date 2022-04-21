using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage.Context.MSSQL
{
    internal partial class MSSQLContext : DbContext
    {
        internal virtual DbSet<Field> Fields { get; set; }

        private static void FieldModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Field>(entity =>
            {
                entity.ToTable("field");

                entity.HasIndex(e => e.BunchId)
                    .HasDatabaseName("fk_field_bunch_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint");

                entity.Property(e => e.BunchId)
                    .HasColumnName("bunch_id")
                    .HasColumnType("bigint");

                entity.HasIndex(e => new { e.Name, e.BunchId })
                    .IsUnique()
                    .HasDatabaseName("idx_field_name");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("nvarchar(128)");

                entity.Property(e => e.FieldType)
                    .IsRequired()
                    .HasColumnName("field_type")
                    .HasColumnType("smallint");

                entity.Property(e => e.DeletedFlag)
                    .IsRequired()
                    .HasColumnName("deleted_flag")
                    .HasColumnType("tinyint");

                entity.HasOne(d => d.Bunch)
                    .WithMany(p => p.Fields)
                    .HasForeignKey(d => d.BunchId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_field_bunch");
            });
        }
    }
}
