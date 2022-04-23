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

        public async Task<RequestResult<NodePattern>> NodeSaveAsync(Action<NodePattern> nodePattern)
        {
            var pattern = new NodePattern();
            nodePattern.Invoke(pattern);
            return await NodeSaveAsync(pattern);
        }

        public async Task<RequestResult<NodePattern>> NodeSaveAsync(NodePattern nodePattern)
        {
            var requestResult = new RequestResult<NodePattern>(true);
            bool creatingNode = nodePattern.NodeId < 1;
            Node node;
            try
            {

                ///Check access via token
                BunchToken bunchToken = await context.Set<BunchToken>().FindAsync(nodePattern.BunchId);
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

                node = creatingNode ? await CreateNodeAsync(nodePattern) : await UpdateNodeAsync(nodePattern);

                nodePattern.NodeId = node.Id;
                nodePattern.Number = node.NodeExt.Number;


                List<Stack> stacks = await CreateStackListAsync(nodePattern);

                List<NodePatternItem> nodeItems = await GetNodePatternItemsAsync(stacks);
                nodePattern.Items = nodeItems;
                requestResult.FillDataSet(new() { nodePattern });

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

        private async Task<Node> CreateNodeAsync(NodePattern nodePattern)
        {

            Node node = new();
            var bunchExt = await context.Set<BunchExt>().FindAsync(nodePattern.BunchId);
            bunchExt.Counter++;

            node.NodeExt = new() { Number = bunchExt.Counter };
            node.NodeToken = new() { ForRLS = contextProperty.MasterToken };
            node.BunchId = nodePattern.BunchId;
            var dateCreated = nodePattern.DateCreated == DateTime.MinValue ? DateTime.UtcNow : nodePattern.DateCreated;
            node.DateCreated = dateCreated;
            node.CreatorInfo = nodePattern.CreatorInfo ?? "unknown";
            node.DeletedFlag = FlagSign.False;

            await context.AddAsync(node);
            await context.SaveChangesAsync();

            return node;
        }

        private async Task<Node> UpdateNodeAsync(NodePattern nodePattern)
        {
            Node node = await context.Set<Node>().FindAsync(nodePattern.NodeId);
            node.BunchId = nodePattern.BunchId;
            node.DeletedFlag = FlagSign.False;
            await context.SaveChangesAsync();

            return node;
        }

        private async Task<List<Stack>> CreateStackListAsync(NodePattern nodePattern)
        {
            List<Stack> stacks;

            if (nodePattern.Items is not null && nodePattern.Items.Count > 0)
            {
                stacks = new();
                nodePattern.Items.ForEach(nodeItem =>
                {
                    var stack = CreateStack(nodePattern.NodeId, nodeItem);
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
                    .Where(x => x.NodeId == nodePattern.NodeId)
                    .ToListAsync();
            }

            return stacks;

        }

        private static Stack CreateStack(long nodeId, NodePatternItem nodeItem)
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
                var value = (List<NodePattern>)nodeItem.Data;
                value.ForEach(nodePattern => {
                    stack.StackLists.Add(new StackList { StackId = stack.Id, NodeId = nodePattern.NodeId });
                });                
                
            }

            if (nodeItem.FieldType == FieldType.File)
            {
                var fileData = (FileData)nodeItem.Data;
                stack.StackFile = new StackFile
                {
                    StackId = stack.Id,
                    FileName = fileData.Name,
                    FileSize = fileData.Size,
                    FileType = fileData.Mime,
                    Data = fileData.ByteArray
                };
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
