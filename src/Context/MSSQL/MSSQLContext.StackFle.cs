﻿using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;

namespace MtdKey.Storage.Context.MSSQL
{
    internal partial class MSSQLContext : DbContext
    {
        internal virtual DbSet<StackFile> StackFiles { get; set; }

        private static void StackFileModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StackFile>(entity =>
            {
                entity.ToTable("stack_file");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasColumnName("id")
                    .HasColumnType("bigint");

                entity.Property(e => e.StackId)
                    .IsRequired()
                    .HasColumnName("stack_id")
                    .HasColumnType("bigint");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasColumnName("file_name")
                    .HasColumnType("nvarchar(256)");

                entity.Property(e => e.FileSize)
                    .HasColumnName("file_size")
                    .HasColumnType("bigint");

                entity.Property(e => e.FileType)
                    .IsRequired()
                    .HasColumnName("file_type")
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.Data)
                    .IsRequired()
                    .HasColumnName("data")
                    .HasColumnType("varbinary(max)");

                entity.HasOne(d => d.Stack)
                    .WithMany(p => p.StackFiles)
                    .HasForeignKey(d => d.StackId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_stack_file");
            });

        }
    }
}
