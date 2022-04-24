using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace MtdKey.Storage.Context.MySQL
{
    internal partial class MySQLContext : DbContext
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

                entity.Property(e => e.DateCreated)
                    .IsRequired()
                    .HasColumnName("date_created")
                    .HasColumnType("DateTime");

                entity.Property(e => e.CreatorInfo)
                    .IsRequired()
                    .HasColumnName("creator_info")
                    .HasColumnType("nvarchar(128)");

                entity.Property(e => e.DeletedFlag)
                    .IsRequired()
                    .HasColumnName("deleted_flag")
                    .HasColumnType("tinyint(2)");

                entity.HasOne(node => node.Bunch)
                    .WithMany(bunch => bunch.Nodes)
                    .HasForeignKey(node => node.BunchId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_node_bunch");
            });

        }
    }
}
