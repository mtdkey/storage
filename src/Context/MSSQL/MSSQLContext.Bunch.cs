using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage.Context.MSSQL
{
    internal partial class MSSQLContext : DbContext
    {
       internal virtual DbSet<Bunch> Bunches { get; set; }

        private static void BunchModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bunch>(entity =>
            {
                entity.ToTable("bunch");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint");

                entity.HasIndex(e => e.Name)
                    .IsUnique()
                    .HasDatabaseName("idx_bunch_name");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("nvarchar(128)");

                entity.Property(e => e.DeletedFlag)
                    .IsRequired()
                    .HasColumnName("deleted_flag")
                    .HasColumnType("tinyint");
            });
        }
    }
}
