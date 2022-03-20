using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace MtdKey.Storage.Context.MySQL
{
    internal class MySQLContextFactory : IDesignTimeDbContextFactory<MySQLContext>
    {

        public MySQLContext CreateDbContext(string[] args)
        {
            string databaseGuid = string.Empty;
            if (args is not null && args.Length > 0) { databaseGuid = args[0]; }

            var configuration = ContextConfig.GetConfiguration();
            var connections = configuration.GetSection("ConnectionStrings").AsEnumerable()
                .Where(x => x.Key.Contains(databaseGuid))
                .ToList();
            var connectionString = connections.FirstOrDefault(x => x.Value is not null);
            ContextProperty contextProperty = new() { ConnectionString = connectionString.Value };
            return CreateDbContext(contextProperty);

        }

        public static MySQLContext CreateDbContext(ContextProperty contextProperty)
        {
            DbContextOptionsBuilder<MySQLContext> optionsBuilder = new();
            optionsBuilder.UseMySQL(contextProperty.ConnectionString);
#if DEBUG            
            optionsBuilder.EnableSensitiveDataLogging();
#endif            
            return new MySQLContext(optionsBuilder.Options);
        }

    }
}
