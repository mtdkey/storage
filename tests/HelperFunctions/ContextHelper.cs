using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage.Tests.HelperFunctions
{
    public static class ContextHelper
    {
        public static ContextProperty CreateContextProperty(string databaseGuid)
        {
            string connectionString = ContextConfig.GetConnectionString(databaseGuid);
            DatabaseType databaseType = ContextConfig.GetDatabaseType(databaseGuid);

            return new ContextProperty
            {
                DatabaseType = databaseType,
                ConnectionString = connectionString
            };
        }
    }
}
