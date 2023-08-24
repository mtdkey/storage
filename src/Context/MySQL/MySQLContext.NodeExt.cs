using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;

namespace MtdKey.Storage.Context.MySQL
{
    internal partial class MySQLContext : DbContext
    {
        internal virtual DbSet<NodeExt> NodeExts { get; set; }

        private static void NodeExtModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NodeExt>(entity =>
            {
                entity.ToTable("node_ext");

                entity.Property(e => e.NodeId)
                    .HasColumnName("node_id")
                    .HasColumnType("bigint");

                entity.Property(e => e.Number)
                    .HasColumnName("number")
                    .HasColumnType("int(10)");

                entity.HasOne(d => d.Node)
                    .WithOne(p => p.NodeExt)
                    .HasForeignKey<NodeExt>(d => d.NodeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_node_node_ext");
            });

        }
    }
}
