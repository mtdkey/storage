using System.Threading.Tasks;
using Xunit;

namespace MtdKey.Storage.Tests
{
    [Collection("Sequential")]
    public class T010_ContextTests
    {

        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task CreateDataBaseAndMigrationAsync(string guidDatabase)
        {            
            var result = await ContextHandler.CreateNewDatabaseAsync(guidDatabase);  
            Assert.True(result.Success, result.Exception?.Message);
        }

    }
}
