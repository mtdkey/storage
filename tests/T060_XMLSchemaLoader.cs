using MtdKey.Storage.DataMapper;
using MtdKey.Storage.Tests.HelperFunctions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MtdKey.Storage.Tests
{
    [Collection("Sequential")]
    public class T060_XMLSchemaLoader
    {
        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task A_DataLoadToDatabaseAsync(string guidDatabase)
        {

            ContextProperty contextProperty = ContextHelper.CreateContextProperty(guidDatabase);            
            using RequestProvider requestProvider = new(contextProperty);

            var loader = new XMLSchemaLoader<T060_XMLSchemaLoader>(requestProvider);
            
            await loader.DataLoadToDatabaseAsync();


        }
    }
}
