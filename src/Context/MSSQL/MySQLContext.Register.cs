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
        internal virtual DbSet<Register> Registers { get; set; }

        private static void RegisterModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Register>(entity =>
            {
                entity.ToTable("register");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("uniqueidentifier");
                
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("nvarchar(128)");

                entity.Property(e => e.Value)
                    .HasColumnName("vlue")
                    .HasColumnType("nvarchar(256)");
            });

        }
    }
}
