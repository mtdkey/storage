using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;
using System.Collections.Generic;

namespace MtdKey.Storage.Context.MSSQL
{
    internal partial class MSSQLContext : DbContext
    {
        internal virtual DbSet<Node> Nodes { get; set; }

        private static void NodeModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Node>(entity =>
            {
                entity.ToTable("node");

                entity.HasIndex(e => e.BunchId)
                    .HasDatabaseName("fk_node_bunch_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint");

                entity.Property(e => e.BunchId)
                    .HasColumnName("bunch_id")
                    .HasColumnType("bigint");

                entity.Property(e => e.DeletedFlag)
                    .IsRequired()
                    .HasColumnName("deleted_flag")
                    .HasColumnType("tinyint");

                entity.HasOne(d => d.Bunch)
                    .WithMany(p => p.Nodes)
                    .HasForeignKey(d => d.BunchId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_node_bunch");
            });
            
        }
    }
}
