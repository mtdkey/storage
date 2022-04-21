using MtdKey.Storage.Tests.HelperFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MtdKey.Storage.Tests
{
    [Collection("Sequential")]
    public class T930_NodeTests
    {
        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task A_Add_Node(string guidDatabase)
        {
            ContextProperty contextProperty = ContextHandler.GetContextProperty(guidDatabase);
            using RequestProvider requestProvider = new(contextProperty);
            var createdNode = await NodeHelper.CreateAsync(requestProvider);
            Assert.True(createdNode.Success);

            var node = createdNode.DataSet.FirstOrDefault();

            Assert.True((bool)node.Items.Find(x => x.FieldType == FieldType.Boolean).Data == true);
            Assert.True((string)node.Items.Find(x => x.FieldType == FieldType.Text).Data == Common.LongTextValue);
            Assert.True((decimal)node.Items.Find(x => x.FieldType == FieldType.Numeric).Data == Common.NumericValue);
            Assert.True((DateTime)node.Items.Find(x => x.FieldType == FieldType.DateTime).Data == Common.DateTimeValue);
        }

        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task B_Query_Node(string guidDatabase)
        {
            ContextProperty contextProperty = ContextHandler.GetContextProperty(guidDatabase);
            using RequestProvider requestProvider = new(contextProperty);

            var createdNode = await NodeHelper.CreateAsync(requestProvider);
            var nodeBasis = createdNode.DataSet.FirstOrDefault();

            var receivedNode = await requestProvider.NodeQueryAsync(filter =>
            {
                filter.BunchIds.Add(nodeBasis.BunchId);
                filter.SearchText = Common.SplitedWordValue;
            });

            Assert.True(receivedNode.Success);
            Assert.True(receivedNode.DataSet.Count == 1);
            
            var nodeFound = receivedNode.DataSet.FirstOrDefault();

            Assert.True(nodeBasis.NodeId == nodeFound.NodeId);
            Assert.True(nodeBasis.BunchId == nodeFound.BunchId);
            Assert.True(nodeBasis.Number == nodeFound.Number);

            Assert.Equal(nodeBasis.NodeId, nodeFound.NodeId);
            Assert.Equal(nodeBasis.BunchId, nodeFound.BunchId);
            Assert.Equal(nodeBasis.Number, nodeFound.Number);

            Assert.True((bool)nodeFound.Items.Find(x => x.FieldType == FieldType.Boolean).Data == true);
            Assert.True((string)nodeFound.Items.Find(x => x.FieldType == FieldType.Text).Data == Common.LongTextValue);
            Assert.True((decimal)nodeFound.Items.Find(x => x.FieldType == FieldType.Numeric).Data == Common.NumericValue);
            Assert.True((DateTime)nodeFound.Items.Find(x => x.FieldType == FieldType.DateTime).Data == Common.DateTimeValue);
        }

        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task C_Delete_Node(string guidDatabase)
        {
            ContextProperty contextProperty = ContextHandler.GetContextProperty(guidDatabase);
            using RequestProvider requestProvider = new(contextProperty);
            var createdNode = await NodeHelper.CreateAsync(requestProvider);
            var node = createdNode.DataSet.FirstOrDefault();

            var deleteResult = await requestProvider.NodeDeleteAsync(node.NodeId);
            Assert.True(deleteResult.Success);

            var operationCheck = await requestProvider.NodeQueryAsync(filter => filter.Ids.Add(node.NodeId));
            Assert.True(operationCheck.Success);
            Assert.True(operationCheck.DataSet.Count == 0);
        }

        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task D_List_Node(string guidDatabase)
        {
            ContextProperty contextProperty = ContextHandler.GetContextProperty(guidDatabase);
            using RequestProvider requestProvider = new(contextProperty);

            //Create a catalog bunch
            var bunchList = await requestProvider.CreateBunchAsync();
            var fieldList = await requestProvider.CreateFieldAsync(bunchList.BunchId, FieldType.Text);

            //Create a bunch to link to the catalog bunch
            var bunchSelector = await requestProvider.CreateBunchAsync();
            var fieldSelector = await requestProvider.CreateFieldAsync(bunchSelector.BunchId, FieldType.Link, bunchList.BunchId);
           


            //Create data items for the catalog bunch
            List<NodeSchemaItem> nodeItems1 = new()
            {
                new NodeSchemaItem("Catalog Item one", fieldList.FieldId, "Tester", DateTime.UtcNow),
            };

            List<NodeSchemaItem> nodeItems2 = new()
            {
                new NodeSchemaItem("Catalog Item two", fieldList.FieldId, "Tester", DateTime.UtcNow),
            };

            //Add a data node to the catalog bunch
            var createdfirstdNode = await requestProvider.NodeSaveAsync(node =>
            {                
                node.BunchId = bunchList.BunchId;
                node.Items = nodeItems1;
            });
            var catalogFirstNode = createdfirstdNode.DataSet.FirstOrDefault();

            var createdSecondNode = await requestProvider.NodeSaveAsync(node =>
            {
                node.BunchId = bunchList.BunchId;
                node.Items = nodeItems2;
            });

            //Create data items for the bunch selector
            List<NodeSchemaItem> selectedNode = new()
            {
                new NodeSchemaItem(catalogFirstNode, fieldSelector.FieldId, "Tester", DateTime.UtcNow),
            };

            //Save selecting list node to database
            var createdNodeSelected = await requestProvider.NodeSaveAsync(node =>
            {
                node.BunchId = bunchSelector.BunchId;
                node.Items = selectedNode;
            });

            Assert.True(createdNodeSelected.Success);

            //Get data from database
            var bunchReceiver = await requestProvider.NodeQueryAsync(filter =>
            {
                filter.BunchIds.Add(bunchSelector.BunchId);
            });

            //Get a complete copy of the List node      
            var nodeReceiver = (NodeSchema)bunchReceiver.DataSet[0].Items.FirstOrDefault(x => x.FieldId == fieldSelector.FieldId).Data;

            Assert.Equal(nodeReceiver.NodeId, catalogFirstNode.NodeId);
            Assert.Equal(nodeReceiver.Number, catalogFirstNode.Number);
            Assert.True((string)nodeReceiver.Items.FirstOrDefault().Data == "Catalog Item one");
        }

        [Theory]
        [InlineData("mssql_test")]
        [InlineData("mysql_test")]
        public async Task E_Change_RLS_Token(string guidDatabase)
        {
            ContextProperty contextProperty = ContextHandler.GetContextProperty(guidDatabase);

            contextProperty.MasterToken = Guid.NewGuid().ToString();
            contextProperty.AccessTokens = new List<string> { string.Empty, contextProperty.MasterToken };

            using RequestProvider requestProvider = new(contextProperty);

            var createdNode = await NodeHelper.CreateAsync(requestProvider);
            var node = createdNode.DataSet.FirstOrDefault();

            var requestRequest = await requestProvider.NodeQueryAsync(filter => { filter.Ids.Add(node.NodeId); });
            Assert.True(requestRequest.Success);
            Assert.True(requestRequest.DataSet.Count == 1);

            contextProperty.MasterToken = Guid.NewGuid().ToString();
            contextProperty.AccessTokens = new List<string> { contextProperty.MasterToken };
            
            using RequestProvider requestProvider2 = new(contextProperty);

            var changeResult = await requestProvider2.NodeChangeTokenForRLS(contextProperty.MasterToken, Guid.NewGuid().ToString());
            Assert.True(changeResult.Success);

            var operationCheck = await requestProvider2.NodeQueryAsync(filter => { filter.Page = 1; });
            Assert.True(operationCheck.Success);
            Assert.True(operationCheck.DataSet.Count == 0);

        }

    }
}
