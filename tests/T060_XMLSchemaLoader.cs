using MtdKey.Storage.Tests.HelperFunctions;
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
        public async Task A_UploadData(string guidDatabase)
        {
            ContextProperty contextProperty = ContextHelper.CreateContextProperty(guidDatabase);            
            using RequestProvider requestProvider = new(contextProperty);

            await requestProvider.BeginTransactionAsync();
            var xmlSchema = new XmlSchema<T060_XMLSchemaLoader>();
            xmlSchema.LoadSchemaFromServer();
            var xmlDoc = xmlSchema.GetXmlDocument();

            var bunchTags = xmlSchema.GetBunches();
            var uploadBunches = await requestProvider.UpLoadBunches(bunchTags);
            
            var fieldTags = xmlSchema.GetFields();
            var uploadFields = await requestProvider.UpLoadFields(fieldTags);

            await requestProvider.CommitTransactionAsync();
            
            Assert.True(uploadBunches.Success, uploadBunches.Exception?.Message);
            Assert.True(uploadFields.Success, uploadFields.Exception?.Message);

        }
    }
}
