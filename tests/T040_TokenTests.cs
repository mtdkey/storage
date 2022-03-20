using MtdKey.Storage.Tests.HelperFunctions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MtdKey.Storage.Tests
{
    [Collection("Sequential")]
    public class T040_TokenTests
    {
        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task A_Set_BunchToken(string guidDatabase)
        {
            var tokenCreate = Guid.NewGuid().ToString();
            var tokenEdit = Guid.NewGuid().ToString();
            var tokenDelete = Guid.NewGuid().ToString();

            ContextProperty contextProperty = ContextHelper.CreateContextProperty(guidDatabase);            
            using RequestProvider requestProvider = new(contextProperty);
            var bunch = await requestProvider.CreateBunchAsync();
                        
            var createResult = await requestProvider.BunchSetTokensAsync(TokenAction.ToCreate, bunch.BunchId, tokenCreate);
            Assert.True(createResult.Success);

            var editResult = await requestProvider.BunchSetTokensAsync(TokenAction.ToEdit, bunch.BunchId, tokenEdit);
            Assert.True(editResult.Success);

            var deleteResult = await requestProvider.BunchSetTokensAsync(TokenAction.ToDelete, bunch.BunchId, tokenDelete);
            Assert.True(deleteResult.Success);

            var createdNode = await NodeHelper.CreateAsync(requestProvider,bunch);
            Assert.False(createdNode.Success);

        }
    }
}
