using System.Threading.Tasks;

namespace MtdKey.Storage.Tests
{
    public class ContextHandler
    {
        public static async Task<IRequestResult> CreateNewDatabaseAsync(string guidDatabase)
        {            
            var contextProperty = GetContextProperty(guidDatabase);

            using RequestProvider requestProvider = new(contextProperty);
            await requestProvider.DeleteDatabaseAsync();
            var result = await requestProvider.MigrationAsync();
            
            return result;
        }

        public static ContextProperty  GetContextProperty(string guidDatabase)
        {
            string connectionString = ContextConfig.GetConnectionString(guidDatabase);
            DatabaseType databaseType = ContextConfig.GetDatabaseType(guidDatabase);

            var contextProperty = new ContextProperty
            {
                DatabaseType = databaseType,
                ConnectionString = connectionString
            };

            return contextProperty;
        }
    }
}
