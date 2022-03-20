using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage.Context
{
    internal static class ContextConfig
    {
        public static string MySQLPrefix => "mysql_";
        public static string MSSQLPrefix => "mssql_";

        public static IConfigurationRoot GetConfiguration()
        {
            var environmentName = Environment.GetEnvironmentVariable("Hosting:Environment");
            var basePath = AppContext.BaseDirectory;

            var builder = new ConfigurationBuilder()
                     .SetBasePath(basePath)
                     .AddJsonFile("storage.json")
                     .AddJsonFile($"storage.{environmentName}.json", true)
                     .AddEnvironmentVariables();

            var configuration = builder.Build();
            return configuration;
        }

        public static string GetConnectionString(string databaseGuid = default)
        {
            var configuration = GetConfiguration();
            var connectionStrings = configuration.GetSection("ConnectionStrings").AsEnumerable();
            if (databaseGuid != default) { connectionStrings = connectionStrings.Where(x => x.Key.Contains(databaseGuid)); }
            var connectionString = connectionStrings.FirstOrDefault(x => x.Value is not null);
            return connectionString.Value;
        }

        public static DatabaseType GetDatabaseType (string databaseGuid = default)
        {
            var configuration = GetConfiguration();
            var connectionStrings = configuration.GetSection("ConnectionStrings").AsEnumerable();
            if (databaseGuid != default) { connectionStrings = connectionStrings.Where(x => x.Key.Contains(databaseGuid)); }
            var connectionString = connectionStrings.FirstOrDefault(x=>x.Value is not null);

            if (connectionString.Key.Contains(MySQLPrefix)) { return DatabaseType.MySQL; }
            if (connectionString.Key.Contains(MSSQLPrefix)) { return DatabaseType.MSSQL; }

            return default;
        }
    }
}
