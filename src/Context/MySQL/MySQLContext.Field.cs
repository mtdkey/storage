using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;


namespace MtdKey.Storage.Context.MySQL
{
    internal partial class MySQLContext : DbContext
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
                    .HasColumnType("tinyint(2)");

                entity.Property(e => e.DeletedFlag)
                    .IsRequired()
                    .HasColumnName("deleted_flag")
                    .HasColumnType("tinyint(2)");

                entity.HasOne(field => field.Bunch)
                    .WithMany(bunch => bunch.Fields)
                    .HasForeignKey(field => field.BunchId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_field_bunch");
            });

            modelBuilder.Entity<Field>().HasQueryFilter(p => p.DeletedFlag == 0);

        }
    }
}
