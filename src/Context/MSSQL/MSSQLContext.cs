using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MtdKey.Storage.Context.MSSQL
{
    internal partial class MSSQLContext : DbContext
    {
        public MSSQLContext() { }
        public MSSQLContext(DbContextOptions<MSSQLContext> options) : base(options) {}

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
            SchemaVersionModelCreating(modelBuilder);
            FieldLinkModelCreating(modelBuilder);
        }

    }
}
