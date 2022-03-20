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
                    .HasColumnType("int");

                entity.HasOne(d => d.Node)
                    .WithOne(p => p.NodeExt)
                    .HasForeignKey<NodeExt>(d => d.NodeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_node_node_ext");
            });

        }
    }
}
