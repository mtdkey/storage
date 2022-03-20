using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;


namespace MtdKey.Storage.Context.MSSQL
{
    internal partial class MSSQLContext : DbContext
    {
        internal virtual DbSet<StackText> StackFrames { get; set; }

        private static void StackTextModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StackText>(entity =>
            {
                entity.ToTable("stack_text");

                entity.HasIndex(e => e.StackId)
                    .HasDatabaseName("fk_stack_text_stack_idx");

                entity.HasIndex(e => e.Value)
                    .HasDatabaseName("idx_stack_text_value");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint");

                entity.Property(e => e.StackId)
                    .IsRequired()
                    .HasColumnName("stack_id")
                    .HasColumnType("bigint");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasColumnType("nvarchar(128)");

                entity.HasOne(d => d.Stack)
                    .WithMany(p => p.StackTexts)
                    .HasForeignKey(d => d.StackId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_stack_text");

            });

        }
    }
}
