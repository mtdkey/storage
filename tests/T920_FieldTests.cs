using MtdKey.Storage.Tests.HelperFunctions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MtdKey.Storage.Tests
{
    [Collection("Sequential")]
    public class T920_FieldTests
    {
        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task A_Create_Field(string guidDatabase)
        {
            ContextProperty contextProperty = ContextHandler.GetContextProperty(guidDatabase);
            using RequestProvider requestProvider = new(contextProperty);

            var createdBunch = await BunchHelper.CreateAsync(requestProvider);
            var bunch = createdBunch.DataSet.FirstOrDefault();

            var createdField = await FieldHelper.CreateAsync(requestProvider, bunch.BunchId, FieldType.Text);
            var field = createdField.DataSet.FirstOrDefault();

            Assert.True(createdField.Success);
            Assert.True(field.FieldId > 0);
        }

        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task B_Receive_Field(string guidDatabase)
        {
            string testString = $"search test {DateTime.UtcNow}";
            ContextProperty contextProperty = ContextHandler.GetContextProperty(guidDatabase);
            using RequestProvider requestProvider = new(contextProperty);

            var createdBunch = await BunchHelper.CreateAsync(requestProvider);
            var bunch = createdBunch.DataSet.FirstOrDefault();

            var createdFieldA = await FieldHelper.CreateAsync(requestProvider, bunch.BunchId, FieldType.Text);
            var fieldA = createdFieldA.DataSet.FirstOrDefault();

            var createdFieldB = await FieldHelper.CreateAsync(requestProvider, bunch.BunchId, FieldType.Text);
            var fieldB = createdFieldB.DataSet.FirstOrDefault();

            var createdArchve = await FieldHelper.CreateArchiveAsync(requestProvider, bunch.BunchId, FieldType.Text);
            var fieldArchive = createdArchve.DataSet.FirstOrDefault();

            //Get FieldA by Id
            var receiveFieldA = await requestProvider.FieldQueryAsync(filter =>
            {
                filter.FieldIds.Add(fieldA.FieldId);
            });

            Assert.True(receiveFieldA.Success);
            Assert.True(receiveFieldA.DataSet.Count == 1);
            Assert.True(receiveFieldA.DataSet[0].FieldId == fieldA.FieldId);

            //Get FieldA by Name
            var receiveFieldB = await requestProvider.FieldQueryAsync(filter =>
            {
                filter.SearchText = fieldB.Name;
            });

            Assert.True(receiveFieldB.Success);
            Assert.True(receiveFieldB.DataSet.Count == 1);
            Assert.True(receiveFieldB.DataSet[0].FieldId == fieldB.FieldId);
        }


        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task C_Update_Field(string guidDatabase)
        {
            string testFieldName = $"New field unique name - {Common.GetRandomName()}";
            ContextProperty contextProperty = ContextHandler.GetContextProperty(guidDatabase);
            using RequestProvider requestProvider = new(contextProperty);

            var createdBunch = await BunchHelper.CreateAsync(requestProvider);
            var bunch = createdBunch.DataSet.FirstOrDefault();

            var createdField = await FieldHelper.CreateAsync(requestProvider, bunch.BunchId, FieldType.Text);
            var field = createdField.DataSet.FirstOrDefault();
            field.Name = testFieldName;

            var updatetedFueld = await requestProvider.FieldSaveAsync(field);

            Assert.True(updatetedFueld.Success);

            var receivedField = await requestProvider.FieldQueryAsync(filer =>
            {
                filer.SearchText = testFieldName;
            });

            Assert.True(receivedField.Success);
            Assert.True(receivedField.DataSet.Count == 1);
            Assert.True(receivedField.DataSet[0].Name == field.Name);
        }

        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task D_Delete_Field(string guidDatabase)
        {
            ContextProperty contextProperty = ContextHandler.GetContextProperty(guidDatabase);
            using RequestProvider requestProvider = new(contextProperty);

            var createdBunch = await BunchHelper.CreateAsync(requestProvider);
            var bunch = createdBunch.DataSet.FirstOrDefault();

            var createdField = await FieldHelper.CreateAsync(requestProvider, bunch.BunchId, FieldType.Text);
            var field = createdField.DataSet.FirstOrDefault();

            var requestResult = await requestProvider.FieldDeleteAsync(field.FieldId);
            Assert.True(requestResult.Success);

            var operationCheck = await requestProvider.FieldQueryAsync(filter => filter.FieldIds.Add(field.FieldId));
            Assert.True(operationCheck.Success);
            Assert.True(operationCheck.DataSet.Count == 0);
        }

    }
}
