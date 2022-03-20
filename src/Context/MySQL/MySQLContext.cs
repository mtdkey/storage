using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MtdKey.Storage.Context.MySQL
{
    internal partial class MySQLContext : DbContext
    {

        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BunchModelCreating(modelBuilder);
            BunchExtModelCreating(modelBuilder);
            BunchTokenModelCreating(modelBuilder);

            FieldModelCreating(modelBuilder);

            NodeModelCreating(modelBuilder);
            NodeExtModelCreating(modelBuilder);
            NodeTokenModelCreating(modelBuilder);

            StackModelCreating(modelBuilder);
            StackDigitalModelCreating(modelBuilder);
            StackTextModelCreating(modelBuilder);
            StackListModelCreating(modelBuilder);

            RegisterModelCreating(modelBuilder);
        }
    }
}
