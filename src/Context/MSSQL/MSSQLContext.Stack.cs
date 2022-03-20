using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;

namespace MtdKey.Storage.Context.MSSQL
{
    internal partial class MSSQLContext : DbContext
    {
        internal virtual DbSet<Stack> Stacks { get; set; }

        private static void StackModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stack>(entity =>
            {
                entity.ToTable("stack");

                entity.HasIndex(e => e.NodeId)
                    .HasDatabaseName("fk_stack_node_idx");

                entity.HasIndex(e => e.FieldId)
                    .HasDatabaseName("fk_stack_field_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint");

                entity.Property(e => e.NodeId)
                    .IsRequired()
                    .HasColumnName("node_id")
                    .HasColumnType("bigint");

                entity.Property(e => e.FieldId)
                    .IsRequired()
                    .HasColumnName("field_id")
                    .HasColumnType("bigint");

                entity.Property(e => e.DateCreated)
                    .IsRequired()
                    .HasColumnName("date_created")
                    .HasColumnType("DateTime");

                entity.Property(e => e.CreatorInfo)
                    .IsRequired()
                    .HasColumnName("creator_info")
                    .HasColumnType("nvarchar(128)");

                entity.HasOne(d => d.Node)
                    .WithMany(p => p.Stacks)
                    .HasForeignKey(d => d.NodeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_stack_node");

                entity.HasOne(d => d.Field)
                    .WithMany(p => p.Stacks)
                    .HasForeignKey(d => d.FieldId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("fk_stack_field");
            });

        }
    }
}
