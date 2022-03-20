using MtdKey.Storage.Tests.HelperFunctions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MtdKey.Storage.Tests
{
    [Collection("Sequential")]
    public class T050_RegisterTests
    {
        public Guid registerId;

        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task A_All_Operation_Register(string guidDatabase)
        {
                
            ContextProperty contextProperty = ContextHelper.CreateContextProperty(guidDatabase);            
            using RequestProvider requestProvider = new(contextProperty);

            var result = await requestProvider.RegisterSaveAsync(schema => {
                schema.Name = "Test register name.";
                schema.Value = "Test register value.";                
            });

            Assert.True(result.Success);
            Assert.True(result.DataSet[0].RegisterId != Guid.Empty);

            var register = result.DataSet[0];
            string testValue = Guid.NewGuid().ToString();
            register.Value = testValue;


        }

    }
}
