using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;


namespace MtdKey.Storage.Context.MSSQL
{
    internal partial class MSSQLContext : DbContext
    {
        internal virtual DbSet<BunchExt> BunchExts { get; set; }

        private static void BunchExtModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<BunchExt>(entity =>
            {
                entity.ToTable("bunch_ext");

                entity.Property(e => e.BunchId)
                    .HasColumnName("bunch_id")
                    .HasColumnType("bigint");

                entity.Property(e => e.Counter)
                    .IsRequired()
                    .HasColumnName("counter")
                    .HasColumnType("int");

                entity.HasOne(d => d.Bunch)
                    .WithOne(p => p.BunchExt)
                    .HasForeignKey<BunchExt>(d => d.BunchId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_bunch_bunch_ext");

            });

        }
    }
}
