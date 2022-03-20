using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;


namespace MtdKey.Storage.Context.MSSQL
{
    internal partial class MSSQLContext : DbContext
    {
        internal virtual DbSet<StackList> StackLists { get; set; }

        private static void StackListModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StackList>(entity =>
            {
                entity.ToTable("stack_list");

                entity.HasIndex(e => e.NodeId)
                    .HasDatabaseName("fk_node_stack_list_idx");

                entity.Property(e => e.StackId)
                    .HasColumnName("number")
                    .HasColumnType("bigint");

                entity.Property(e => e.NodeId)
                    .HasColumnName("node_id")
                    .HasColumnType("bigint");                

                entity.HasOne(d => d.Stack)
                    .WithOne(p => p.StackList)
                    .HasForeignKey<StackList>(d => d.StackId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_stack_list");

                entity.HasOne(d => d.Node)
                    .WithMany(p => p.StackLists)
                    .HasForeignKey(d => d.NodeId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("fk_node_stack_list");

            });

        }
    }
}
