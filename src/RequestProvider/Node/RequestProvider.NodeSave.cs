using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;
using MtdKey.Storage.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public partial class RequestProvider : IDisposable
    {

        public async Task<RequestResult<NodeSchema>> NodeSaveAsync(Action<NodeSchema> nodeSchema)
        {
            var schema = new NodeSchema();
            nodeSchema.Invoke(schema);
            return await NodeSaveAsync(schema);
        }

        public async Task<RequestResult<NodeSchema>> NodeSaveAsync(NodeSchema nodeSchema)
        {
            var requestResult = new RequestResult<NodeSchema>(true);
            bool creatingNode = nodeSchema.NodeId < 1;
            Node node;
            try
            {

                ///Check access via token
                BunchToken bunchToken = await context.Set<BunchToken>().FindAsync(nodeSchema.BunchId);
                bool creatingAllowed = contextProperty.AccessTokens.Contains(bunchToken.TokenToCreate);
                bool editingAllowed = contextProperty.AccessTokens.Contains(bunchToken.TokenToEdit);

                if (creatingNode is true && creatingAllowed is not true)
                {
                    requestResult.SetResultInfo(false, new Exception("Access denied!"));
                    return requestResult;
                }

                if (creatingNode is false && editingAllowed is not true)
                {
                    requestResult.SetResultInfo(false, new Exception("Access denied!"));
                    return requestResult;
                }

                node = creatingNode ? await CreateNodeAsync(nodeSchema) : await UpdateNodeAsync(nodeSchema);

                nodeSchema.NodeId = node.Id;
                nodeSchema.Number = node.NodeExt.Number;

                List<Stack> stacks = await CreateStackListAsync(nodeSchema);

                List<NodeSchemaItem> nodeItems = await GetNodeSchemaItemsAsync(stacks);
                nodeSchema.Items = nodeItems;
                requestResult.FillDataSet(new() { nodeSchema });

            }
            catch (Exception exception)
            {
                requestResult.SetResultInfo(false, exception);
#if DEBUG
                throw;
#endif
            }

            return requestResult;
        }

        private async Task<Node> CreateNodeAsync(NodeSchema nodeSchema)
        {

            Node node = new();
            var bunchExt = await context.Set<BunchExt>().FindAsync(nodeSchema.BunchId);
            bunchExt.Counter++;

            node.NodeExt = new() { Number = bunchExt.Counter };
            node.NodeToken = new() { ForRLS = contextProperty.MasterToken };

            node.BunchId = nodeSchema.BunchId;
            node.DeletedFlag = FlagSign.False;

            await context.AddAsync(node);
            await context.SaveChangesAsync();

            return node;
        }

        private async Task<Node> UpdateNodeAsync(NodeSchema nodeSchema)
        {
            Node node = await context.Set<Node>().FindAsync(nodeSchema.NodeId);
            node.BunchId = nodeSchema.BunchId;
            node.DeletedFlag = FlagSign.False;
            await context.SaveChangesAsync();

            return node;
        }

        private async Task<List<Stack>> CreateStackListAsync(NodeSchema nodeSchema)
        {
            List<Stack> stacks;

            if (nodeSchema.Items is not null && nodeSchema.Items.Count > 0)
            {
                stacks = new();
                nodeSchema.Items.ForEach(nodeItem =>
                {
                    var stack = CreateStack(nodeSchema.NodeId, nodeItem);
                    stacks.Add(stack);
                });

                await context.AddRangeAsync(stacks);
                await context.SaveChangesAsync();
            }
            else
            {
                var scriptGetMaxIds = SqlScript.StackMaxIds(contextProperty.DatabaseType);
                stacks = await context.Set<Stack>()
                    .FromSqlRaw(scriptGetMaxIds)
                    .Where(x => x.NodeId == nodeSchema.NodeId)
                    .ToListAsync();
            }

            return stacks;

        }

        private static Stack CreateStack(long nodeId, NodeSchemaItem nodeItem)
        {
            var stack = new Stack()
            {
                NodeId = nodeId,
                FieldId = nodeItem.FieldId,
                DateCreated = DateTime.UtcNow,
                CreatorInfo = nodeItem.CreatorInfo,
            };

            if (nodeItem.FieldType == FieldType.Text)
            {
                var stackText = new List<StackText>();
                var datas = SplitText((string)nodeItem.Data);
                foreach (KeyValuePair<long, string> data in datas)
                {
                    var frame = new StackText()
                    {
                        StackId = stack.Id,
                        Value = data.Value
                    };

                    stackText.Add(frame);
                }

                stack.StackTexts = stackText.Count > 0 ? stackText : null;
            }

            if (nodeItem.FieldType == FieldType.Numeric)
            {
                var value = (decimal)nodeItem.Data;
                stack.StackDigital = new StackDigital { StackId = stack.Id, Value = value };
            }

            if (nodeItem.FieldType == FieldType.DateTime)
            {
                var dateTime = (DateTime)nodeItem.Data;
                decimal value = dateTime.Ticks;
                stack.StackDigital = new StackDigital { StackId = stack.Id, Value = value };
            }

            if (nodeItem.FieldType == FieldType.Boolean)
            {
                var value = (bool)nodeItem.Data;
                stack.StackDigital = new StackDigital { StackId = stack.Id, Value = value ? 1 : 0 };
            }

            if (nodeItem.FieldType == FieldType.Link)
            {
                var value = (NodeSchema)nodeItem.Data;
                stack.StackList = new StackList { StackId = stack.Id, NodeId = value.NodeId };
            }

            return stack;
        }

        private static SortedDictionary<long, string> SplitText(string data)
        {
            SortedDictionary<long, string> list = new();

            int blockSize = 128;
            string textValue = data;
            long counter = 0;
            foreach (var block in textValue.SplitByLength(blockSize))
            {
                counter++;
                list.Add(counter, block);
            }

            return list;
        }

    }
}
