﻿using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;

namespace MtdKey.Storage.Context.MySQL
{
    internal partial class MySQLContext : DbContext
    {
        internal virtual DbSet<NodeToken> NodeTokens { get; set; }

        private static void NodeTokenModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NodeToken>(entity =>
            {
                entity.ToTable("node_token");

                entity.Property(e => e.NodeId)
                    .HasColumnName("node_id")
                    .HasColumnType("bigint");

                entity.HasIndex(e => e.ForRLS)
                    .HasDatabaseName("idx_rls_token");

                entity.Property(e => e.ForRLS)
                    .IsRequired()
                    .HasColumnName("for_rls")
                    .HasColumnType("nvarchar(128)");

                entity.HasOne(d => d.Node)
                    .WithOne(p => p.NodeToken)
                    .HasForeignKey<NodeToken>(d => d.NodeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_node_token_for_rls");
            });

        }
    }
}
