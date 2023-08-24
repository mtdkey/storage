using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;


namespace MtdKey.Storage.Context.MSSQL
{
    internal partial class MSSQLContext : DbContext
    {
        internal virtual DbSet<StackLink> StackLists { get; set; }

        private static void StackLinkModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StackLink>(entity =>
            {
                entity.ToTable("stack_link");

                entity.HasIndex(e => e.NodeId)
                    .HasDatabaseName("fk_node_stack_link_idx");

                entity.HasIndex(e => e.StackId)
                    .HasDatabaseName("fk_stack_link_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint");

                entity.Property(e => e.StackId)
                    .HasColumnName("stack_id")
                    .HasColumnType("bigint");

                entity.Property(e => e.NodeId)
                    .HasColumnName("node_id")
                    .HasColumnType("bigint");

                entity.HasOne(d => d.Stack)
                    .WithMany(p => p.StackLists)
                    .HasForeignKey(d => d.StackId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_stack_link");

                entity.HasOne(d => d.Node)
                    .WithMany(p => p.StackLists)
                    .HasForeignKey(d => d.NodeId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("fk_node_stack_link");

            });

        }
    }
}
