using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage.Context.MySQL
{
    internal partial class MySQLContext : DbContext
    {
        internal virtual DbSet<StackList> StackCatalogs { get; set; }

        private static void StackListModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StackList>(entity =>
            {
                entity.ToTable("stack_list");

                entity.HasIndex(e => e.NodeId)
                    .HasDatabaseName("fk_node_stack_list_idx");

                entity.HasIndex(e => e.StackId)
                    .HasDatabaseName("fk_stack_list_idx");

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
