using System.Collections.Generic;
using System.Linq;
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

            ContextProperty contextProperty = ContextHandler.GetContextProperty(guidDatabase);
            using RequestProvider requestProvider = new(contextProperty);

            var schemas = new List<IXmlSchema>();
            var schemaNames = new[] { "Issue", "User" };
            foreach (var schemaName in schemaNames)
            {
                var schema = new XmlSchema<T020_XMLSchema>();
                schema.LoadSchemaFromServer(schemaName);
                schemas.Add(schema);
            }                       

            var uploadResult = await requestProvider.UploadSchemaAsync(schemas);

            Assert.True(uploadResult.Success, uploadResult.Exception?.Message);

            var bunchFieldsReturned = await requestProvider.GetAllBunchesAndFieldsAsync();
            Assert.True(bunchFieldsReturned.Success, bunchFieldsReturned.Exception?.Message);
            Assert.True(bunchFieldsReturned.DataSet.Where(x => x.BunchPattern.Name == "Issue").Any());
            Assert.True(bunchFieldsReturned.DataSet.Where(x => x.BunchPattern.Name == "User").Any());
            Assert.True(bunchFieldsReturned.DataSet.Where(x => x.BunchPattern.Name == "IssueCategory").Any());

            var fieldExists = bunchFieldsReturned.DataSet.Where(x => x.BunchPattern.Name == "Issue" 
                    && x.FieldPatterns.Where(x => x.Name == "AssignedTo").Any()).Any();
            
            Assert.True(fieldExists);

            bunchFieldsReturned = await requestProvider.GetBunchFieldsAsync("Issue");
            Assert.True(bunchFieldsReturned.Success, bunchFieldsReturned.Exception?.Message);
            Assert.True(bunchFieldsReturned.DataSet.Where(x => x.BunchPattern.Name == "Issue").Any());

            fieldExists = bunchFieldsReturned.DataSet.Where(x => x.BunchPattern.Name == "Issue"
                    && x.FieldPatterns.Where(x => x.Name == "AssignedTo").Any()).Any();

            Assert.True(fieldExists);

            //Check Unique Value - needs to be refined
            var values = new Dictionary<string, object>() {
                { "Name", "UniqueData" },
                { "Email", "user@example.com" }
            };

            var userCreated = await requestProvider.NodeCreateAsync("User", values, "Tester");
            Assert.True(userCreated.Success);

            var userSchema = await requestProvider.GetBunchFieldsAsync("User");

            var field = userSchema.DataSet[0].FieldPatterns.FirstOrDefault(x => x.Name == "Name");
            field ??= new();

            var nodeReturned = await requestProvider.NodeQueryAsync(filter => {
                filter.SearchText = "UniqueData";
                filter.BunchNames.Add("User");
                filter.FieldIds.Add(field.FieldId);
            });

            Assert.True(nodeReturned.Success);
            Assert.True(nodeReturned.DataSet[0].Items.Count > 0);
        }
    }
}
