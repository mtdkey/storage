using MtdKey.Storage.Tests.HelperFunctions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MtdKey.Storage.Tests
{
    [Collection("Sequential")]
    public class T910_BunchTests
    {

        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task Context(string guidDatabase)
        {
            ContextProperty contextProperty = ContextHandler.GetContextProperty(guidDatabase);
            using RequestProvider requestProvider = new(contextProperty);

            var requestResult = await BunchHelper.CreateAsync(requestProvider);

            Assert.True(requestResult.Success);
            Assert.True(requestResult.DataSet.FirstOrDefault().BunchId > 0);
        }

        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task A_Create_Bunch(string guidDatabase)
        {
            ContextProperty contextProperty = ContextHandler.GetContextProperty(guidDatabase);
            using RequestProvider requestProvider = new(contextProperty);

            var requestResult = await BunchHelper.CreateAsync(requestProvider);

            Assert.True(requestResult.Success);
            Assert.True(requestResult.DataSet.FirstOrDefault().BunchId > 0);
        }


        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task A1_Create_And_Recieve_Bunch(string guidDatabase)
        {
            ContextProperty contextProperty = ContextHandler.GetContextProperty(guidDatabase);
            using RequestProvider requestProvider = new(contextProperty);

            var requestResult = await BunchHelper.CreateAsync(requestProvider);
            var bunch = requestResult.DataSet.FirstOrDefault();

            Assert.True(requestResult.Success);
            Assert.True(requestResult.DataSet.FirstOrDefault().BunchId > 0);

            var receivedbunch = await requestProvider.BunchQueryAsync(filter =>
            {
                filter.BunchIds.Add(bunch.BunchId);
            });

            Assert.True(receivedbunch.Success);
            Assert.True(receivedbunch.DataSet.Count == 1);

        }

        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task B_Receive_Bunch(string guidDatabase)
        {
            ContextProperty contextProperty = ContextHandler.GetContextProperty(guidDatabase);
            using RequestProvider requestProvider = new(contextProperty);

            //Create BunchA
            var createdBunchA = await BunchHelper.CreateAsync(requestProvider);
            //Create BunchB
            var createdBunchB = await BunchHelper.CreateAsync(requestProvider);
            //Create archive bunch
            var createdArchive = await BunchHelper.CreateArchiveAsync(requestProvider);

            Assert.True(createdBunchA.Success);
            Assert.True(createdBunchB.Success);
            Assert.True(createdArchive.Success);

            //Get BunchA by Id
            var receivedbunchA = await requestProvider.BunchQueryAsync(filter =>
            {
                filter.BunchIds.Add(createdBunchA.DataSet[0].BunchId);
            });

            Assert.True(receivedbunchA.Success);
            Assert.True(receivedbunchA.DataSet.Count == 1);

            //Get BunchB By Name
            string nameB = createdBunchB.DataSet.FirstOrDefault().Name;
            var receivedBunchB = await requestProvider.BunchQueryAsync(filer =>
            {
                filer.SearchText = nameB;
            });

            Assert.True(receivedBunchB.Success);
            Assert.True(receivedBunchB.DataSet.Count == 1);

        }

        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task C_Update_Bunch(string guidDatabase)
        {
            string testBunchName = $"New bunch unique name - {Common.GetRandomName()}";
            ContextProperty contextProperty = ContextHandler.GetContextProperty(guidDatabase);
            using RequestProvider requestProvider = new(contextProperty);

            var createdBunch = await BunchHelper.CreateAsync(requestProvider);
            var bunch = createdBunch.DataSet.FirstOrDefault();
            bunch.Name = testBunchName;

            var updatedBunch = await requestProvider.BunchSaveAsync(bunch);
            Assert.True(updatedBunch.Success);

            var receivedBunch = await requestProvider.BunchQueryAsync(filer =>
            {
                filer.SearchText = testBunchName;
            });

            Assert.True(receivedBunch.Success);
            Assert.True(receivedBunch.DataSet.Count == 1);
            Assert.True(receivedBunch.DataSet[0].Name == bunch.Name);
        }

        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task D_Delete_Bunch(string guidDatabase)
        {
            ContextProperty contextProperty = ContextHandler.GetContextProperty(guidDatabase);
            using RequestProvider requestProvider = new(contextProperty);

            var createdBunch = await BunchHelper.CreateAsync(requestProvider);
            var bunch = createdBunch.DataSet.FirstOrDefault();

            var requestResult = await requestProvider.BunchDeleteAsync(bunch.BunchId);
            Assert.True(requestResult.Success);

            var operationCheck = await requestProvider.BunchQueryAsync(filter => filter.BunchIds.Add(bunch.BunchId));
            Assert.True(operationCheck.Success);
            Assert.True(operationCheck.DataSet.Count == 0);
        }

    }
}
