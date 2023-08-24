using Microsoft.EntityFrameworkCore;

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
            FieldLinkModelCreating(modelBuilder);

            NodeModelCreating(modelBuilder);
            NodeExtModelCreating(modelBuilder);
            NodeTokenModelCreating(modelBuilder);

            StackModelCreating(modelBuilder);
            StackDigitalModelCreating(modelBuilder);
            StackTextModelCreating(modelBuilder);
            StackLinkModelCreating(modelBuilder);

            StackFileModelCreating(modelBuilder);

            SchemaNameModelCreating(modelBuilder);
            SchemaVersionModelCreating(modelBuilder);
        }
    }
}
