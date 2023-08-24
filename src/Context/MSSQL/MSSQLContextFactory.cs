using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace MtdKey.Storage.Context.MSSQL
{
    internal class MSSQLContextFactory : IDesignTimeDbContextFactory<MSSQLContext>
    {

        public MSSQLContext CreateDbContext(string[] args)
        {
            string databaseGuid = string.Empty;
            if (args is not null && args.Length > 0) { databaseGuid = args[0]; }

            var configuration = ContextConfig.GetConfiguration();
            var connections = configuration.GetSection("ConnectionStrings").AsEnumerable()
                .Where(x => x.Key.Contains(databaseGuid))
                .ToList();
            var connectionString = connections.FirstOrDefault(x => x.Value is not null && x.Key.Contains(ContextConfig.MSSQLPrefix));
            ContextProperty contextProperty = new() { ConnectionString = connectionString.Value };
            return CreateDbContext(contextProperty);
        }

        public static MSSQLContext CreateDbContext(ContextProperty contextProperty)
        {
            DbContextOptionsBuilder<MSSQLContext> optionsBuilder = new();
            optionsBuilder.UseSqlServer(contextProperty.ConnectionString);
#if DEBUG            
            optionsBuilder.EnableSensitiveDataLogging();
#endif
            return new MSSQLContext(optionsBuilder.Options);
        }

    }
}
