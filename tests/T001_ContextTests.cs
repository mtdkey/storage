using Microsoft.Extensions.Configuration;
using MtdKey.Storage.Tests.HelperFunctions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MtdKey.Storage.Tests
{
    [Collection("Sequential")]
    public class T001_ContextTests
    {

        [Fact]
        public void A_Create_Context()
        {
            // Use the Request Provider by specifying a connection string
            //using RequestProvider requestProvider = new(contextProperty =>
            //{
            //    contextProperty.DatabaseType = DatabaseType.MSSQL;
            //    contextProperty.ConnectionString = "connection string to your MSSQL database";
            //});

            // Use the Request Provider by specifying the storage.json file
            //var dataBaseID = "ID1";
            //var contextProperty = ContextConfig.GetConnectionString(dataBaseID);
            //var databaseType = ContextConfig.GetDatabaseType(dataBaseID);
            //using RequestProvider requestProvider = new(contextProperty =>
            //{
            //    contextProperty.DatabaseType = databaseType;
            //    contextProperty.ConnectionString = connectionString;
            //});

            var configuration = ContextConfig.GetConfiguration();
            var connections = configuration.GetSection("ConnectionStrings").AsEnumerable().Where(x=>x.Value is not null).ToList();

            foreach (var connection in connections)
            {
                var connectionString = connection.Value;
                DatabaseType databaseType = DatabaseType.MSSQL;
                if (connection.Key.Contains("mysql_")) { databaseType = DatabaseType.MySQL; }

                RequestProvider requestProvider = null;                
                try
                {
                    requestProvider = new(contextProperty =>
                    {
                        contextProperty.DatabaseType =  databaseType;
                        contextProperty.ConnectionString = connectionString;
                    });

                }
                catch 
                {
                    throw;
                }
                finally
                {
                    requestProvider.Dispose();
                }
            }

        }


        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task B_New_Database(string guidDatabase)
        {
            ContextProperty contextProperty = ContextHelper.CreateContextProperty(guidDatabase);
            using RequestProvider requestProvider = new(contextProperty);
            await requestProvider.DeleteDatabaseAsync();

            var result = await requestProvider.MigrationAsync();
            Assert.True(result.Success);
        }

    }
}
