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

                entity.HasIndex(e => e.ParentId)
                    .HasDatabaseName("fk_field_bunch_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint");

                entity.Property(e => e.ParentId)
                    .HasColumnName("parent_id")
                    .HasColumnType("bigint");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("nvarchar(128)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("nvarchar(256)");

                entity.Property(e => e.FieldType)
                    .IsRequired()
                    .HasColumnName("field_type")
                    .HasColumnType("smallint");

                entity.Property(e => e.ArchiveFlag)
                    .IsRequired()
                    .HasColumnName("archive_flag")
                    .HasColumnType("tinyint");

                entity.Property(e => e.DeletedFlag)
                    .IsRequired()
                    .HasColumnName("deleted_flag")
                    .HasColumnType("tinyint");

                entity.HasOne(d => d.Bunch)
                    .WithMany(p => p.Fields)
                    .HasForeignKey(d => d.ParentId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_field_bunch");
            });

            modelBuilder.Entity<Field>().HasQueryFilter(p => p.DeletedFlag == 0);

        }
    }
}
