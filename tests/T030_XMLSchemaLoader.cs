using System.Threading.Tasks;
using Xunit;

namespace MtdKey.Storage.Tests
{
    [Collection("Sequential")]
    public class T030_XMLSchemaLoader
    {
        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task UploadDataFromXmlSchema(string guidDatabase)
        {
            var result = await ContextHandler.CreateNewDatabaseAsync(guidDatabase);
            Assert.True(result.Success, result.Exception?.Message);

            ContextProperty contextProperty =  ContextHandler.GetContextProperty(guidDatabase);            
            using RequestProvider requestProvider = new(contextProperty);

            await requestProvider.BeginTransactionAsync();
            var xmlSchema = new XmlSchema<T030_XMLSchemaLoader>("Issue");
            xmlSchema.LoadSchemaFromServer();
            var xmlDoc = xmlSchema.GetXmlDocument();

            var bunchTags = xmlSchema.GetBunches();
            var uploadBunches = await requestProvider.UpLoadSchena(bunchTags);
            
            var fieldTags = xmlSchema.GetFields();
            var uploadFields = await requestProvider.UpLoadSchena(fieldTags);

            await requestProvider.CommitTransactionAsync();
            
            Assert.True(uploadBunches.Success, uploadBunches.Exception?.Message);
            Assert.True(uploadFields.Success, uploadFields.Exception?.Message);

        }
    }
}
