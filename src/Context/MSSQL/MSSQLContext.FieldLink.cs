using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;


namespace MtdKey.Storage.Context.MSSQL
{
    internal partial class MSSQLContext : DbContext
    {
        internal virtual DbSet<FieldLink> FieldLinks { get; set; }

        private static void FieldLinkModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<FieldLink>(entity =>
            {
                entity.ToTable("field_link");

                entity.HasIndex(e => e.BunchId)
                    .HasDatabaseName("fk_field_link_bunch_idx");

                entity.HasIndex(e => e.FieldId)
                    .HasDatabaseName("fk_field_link_field_idx");

                entity.Property(e => e.FieldId)
                    .HasColumnName("field_id")
                    .HasColumnType("bigint");

                entity.Property(e => e.BunchId)
                    .HasColumnName("bunch_id")
                    .HasColumnType("bigint");

                entity.HasOne(fieldLink => fieldLink.Bunch)
                    .WithMany(bunch => bunch.FieldLinks)
                    .HasForeignKey(fieldLink => fieldLink.BunchId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_field_link_bunch");

                entity.HasOne(fieldLink => fieldLink.Field)
                    .WithOne(field => field.FieldLink)
                    .HasForeignKey<FieldLink>(field => field.FieldId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("fk_field_link_field");

            });

        }
    }
}
