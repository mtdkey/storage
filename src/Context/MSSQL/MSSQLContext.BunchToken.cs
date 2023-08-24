using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;


namespace MtdKey.Storage.Context.MSSQL
{
    internal partial class MSSQLContext : DbContext
    {
        internal virtual DbSet<BunchToken> BunchTokens { get; set; }

        private static void BunchTokenModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<BunchToken>(entity =>
            {
                entity.ToTable("bunch_token");

                entity.Property(e => e.BunchId)
                    .HasColumnName("bunch_id")
                    .HasColumnType("bigint");

                entity.Property(e => e.TokenToCreate)
                    .IsRequired()
                    .HasColumnName("token_to_create")
                    .HasColumnType("nvarchar(128)");

                entity.Property(e => e.TokenToEdit)
                    .IsRequired()
                    .HasColumnName("token_to_edit")
                    .HasColumnType("nvarchar(128)");

                entity.Property(e => e.TokenoDelete)
                    .IsRequired()
                    .HasColumnName("token_to_delete")
                    .HasColumnType("nvarchar(128)");

                entity.HasOne(d => d.Bunch)
                    .WithOne(p => p.BunchToken)
                    .HasForeignKey<BunchToken>(d => d.BunchId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_bunch_token");
            });

        }
    }
}
