using MtdKey.Storage.Tests.HelperFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MtdKey.Storage.Tests
{
    [Collection("Sequential")]
    public class T050_ClearSchemas
    {
        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task ClearSchemas(string guidDatabase)
        {
            //Create Database
            await ContextHandler.CreateNewDatabaseAsync(guidDatabase);
            ContextProperty contextProperty = ContextHandler.GetContextProperty(guidDatabase);
            using RequestProvider requestProvider = new(contextProperty);

            //Upload a schema from a configuration file
            var schema = new XmlSchema<T050_ClearSchemas>();
            schema.LoadSchemaFromXml(Common.OldXmlSchema);
            var uploadResult = await requestProvider.UploadSchemaAsync(new() { schema });
            Assert.True(uploadResult.Success);

            //Fill data to database          
            await CreateData(requestProvider, "IssueCategory");
            await CreateData(requestProvider, "Issue");
            var nodesRequest = await requestProvider.NodeQueryAsync(filter => filter.PageSize = int.MaxValue);
            Assert.True(nodesRequest.Success);
            Assert.True(nodesRequest.DataSet.Count > 0);
            Assert.True(nodesRequest.DataSet.FirstOrDefault().Items.Where(x=> x.FieldType.Equals(FieldType.Text) && (string)x.Data == "test field").Any());
            var fieldRequest = await requestProvider.FieldQueryAsync(filter => filter.SearchText = "Category");
            Assert.True(fieldRequest.Success);
            Assert.True(fieldRequest.DataSet.Count>0);
            var bunchRequest = await requestProvider.BunchQueryAsync(filter => filter.SearchText = "IssueReport");
            Assert.True(bunchRequest.Success);
            Assert.True(bunchRequest.DataSet.Count > 0);


            //Upload new schema            
            schema.LoadSchemaFromXml(Common.NewXmlSchema);
            uploadResult = await requestProvider.UploadSchemaAsync(new() { schema });
            Assert.True(uploadResult.Success);

            //Clear all tracking entities
            requestProvider.Dispose();

            //Clear schemas
            using RequestProvider cleaningProvider = new(contextProperty);
            var cleaningResult = await cleaningProvider.ClearSchemas();
            Assert.True(cleaningResult.Success);

            var categoryDeleted =  await cleaningProvider.FieldQueryAsync(filter => filter.SearchText = "Category");
            Assert.True(categoryDeleted.Success);
            Assert.True(categoryDeleted.DataSet.Count==0);

            var reportDeleted = await cleaningProvider.BunchQueryAsync(filter => filter.SearchText = "IssueReport");
            Assert.True(reportDeleted.Success);
            Assert.True(reportDeleted.DataSet.Count == 0);

        }


        private async Task CreateData(RequestProvider requestProvider, string bunchName)
        {
            var bunchFields = await requestProvider.BunchQueryAsync(filter => filter.BunchNames.Add(bunchName));
            var fieldPatterns = bunchFields.DataSet.FirstOrDefault().FieldPatterns;
            var nodeItems = new List<NodePatternItem>();
            foreach (var fieldPattern in fieldPatterns)
            {
                if (fieldPattern.FieldType == FieldType.Text)
                    nodeItems.Add(new NodePatternItem("test field", fieldPattern.FieldId, "Tester", DateTime.UtcNow));

                if (fieldPattern.FieldType == FieldType.DateTime)
                    nodeItems.Add(new NodePatternItem(DateTime.UtcNow, fieldPattern.FieldId, "Tester", DateTime.UtcNow));

                if (fieldPattern.FieldType == FieldType.Boolean)
                    nodeItems.Add(new NodePatternItem(true, fieldPattern.FieldId, "Tester", DateTime.UtcNow));

                if (fieldPattern.FieldType == FieldType.LinkSingle)
                {
                   var category =  await requestProvider.NodeQueryAsync(filter => filter.BunchNames.Add("IssueCategory"));
                    nodeItems.Add(new NodePatternItem(category.DataSet, fieldPattern.FieldId, "Tester", DateTime.UtcNow));
                }                    
            }

            await requestProvider.NodeSaveAsync(nodePattern => {
                nodePattern.BunchId = bunchFields.DataSet.First().BunchId;
                nodePattern.DateCreated = DateTime.UtcNow;
                nodePattern.CreatorInfo = "Tester";
                nodePattern.Items = nodeItems;
            });
        }
    }
}
