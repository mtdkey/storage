using System;
using System.Threading.Tasks;
using Xunit;

namespace MtdKey.Storage.Tests
{
    [Collection("Sequential")]
    public class T040_CreateSchema
    {

        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task A_CreateBunchAndFields(string guidDatabase)
        {
            await ContextHandler.CreateNewDatabaseAsync(guidDatabase);

            ContextProperty contextProperty = ContextHandler.GetContextProperty(guidDatabase);
            using RequestProvider requestProvider = new(contextProperty);

            var index = DateTime.Now.Ticks;
            var result = await requestProvider.TestCreateBunchAndFieldsAsync(index);

            Assert.True(result.Success, result.Exception?.Message);

        }

    }
}
