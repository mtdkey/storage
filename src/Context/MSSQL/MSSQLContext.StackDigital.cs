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
        internal virtual DbSet<StackDigital> StackDigitals { get; set; }

        private static void StackDigitalModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StackDigital>(entity =>
            {
                entity.ToTable("stack_digital");

                entity.HasIndex(e => e.Value)
                    .HasDatabaseName("idx_stack_digital_value");

                entity.Property(e => e.StackId)
                    .HasColumnName("stack_id")
                    .HasColumnType("bigint");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasColumnType("decimal(20,2)");

                entity.HasOne(d => d.Stack)
                    .WithOne(p => p.StackDigital)
                    .HasForeignKey<StackDigital>(d => d.StackId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_stack_digital");

            });

        }
    }
}
