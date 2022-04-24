using System;
using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public partial class RequestProvider : IDisposable
    {
        private async Task<List<NodePatternItem>> GetNodePatternItemsAsync(IList<Stack> stacks)
        {
            List<NodePatternItem> nodeItems = new();
            List<long> fieldIds = stacks.GroupBy(x => x.FieldId).Select(x => x.Key).ToList();

            Dictionary<long, int> fieldTypes = await context.Set<Field>()
                .Where(field => fieldIds.Contains(field.Id))
                .Select(s => new { s.Id, s.FieldType })
                .ToDictionaryAsync(d => d.Id, d => d.FieldType);

            foreach (var stack in stacks)
            {

                int fieldType = fieldTypes
                    .Where(x => x.Key.Equals(stack.FieldId)).Select(x => x.Value)
                    .FirstOrDefault();

                if (fieldType == (int)FieldType.Numeric)
                {
                    await context.Entry(stack).Reference(x => x.StackDigital).LoadAsync();
                    var value = stack.StackDigital.Value;
                    NodePatternItem nodeItem = new(value, stack.FieldId, stack.CreatorInfo, stack.DateCreated);
                    nodeItem.NodeId = stack.NodeId;
                    nodeItems.Add(nodeItem);
                }

                if (fieldType == (int)FieldType.Boolean)
                {
                    await context.Entry(stack).Reference(x => x.StackDigital).LoadAsync();
                    bool value = stack.StackDigital.Value == 1;
                    NodePatternItem nodeItem = new(value, stack.FieldId, stack.CreatorInfo, stack.DateCreated);
                    nodeItem.NodeId = stack.NodeId;
                    nodeItems.Add(nodeItem);
                }

                if (fieldType == (int)FieldType.DateTime)
                {
                    await context.Entry(stack).Reference(x => x.StackDigital).LoadAsync();
                    DateTime value = new((long)stack.StackDigital.Value);
                    NodePatternItem nodeItem = new(value, stack.FieldId, stack.CreatorInfo, stack.DateCreated);
                    nodeItem.NodeId = stack.NodeId;
                    nodeItems.Add(nodeItem);
                }

                if (fieldType == (int)FieldType.Text)
                {
                    await context.Entry(stack).Collection(x => x.StackTexts).LoadAsync();
                    List<string> values = stack.StackTexts.Select(x => x.Value).ToList();
                    string value = string.Concat(values);
                    NodePatternItem nodeItem = new(value, stack.FieldId, stack.CreatorInfo, stack.DateCreated);
                    nodeItem.NodeId = stack.NodeId;
                    nodeItems.Add(nodeItem);
                }

                if (fieldType == (int)FieldType.Link)
                {
                    await context.Entry(stack).Collection(x => x.StackLists).LoadAsync();
                    var nodePatterns = new List<NodePattern>();
                    foreach (var stackList in stack.StackLists)
                    {
                        await context.Entry(stackList).Reference(x => x.Node).LoadAsync();

                        var listNode = stackList.Node;
                        await context.Entry(listNode).Collection(x => x.Stacks).LoadAsync();
                        await context.Entry(listNode).Reference(x => x.NodeExt).LoadAsync();

                        var catalogStacks = listNode.Stacks.ToList();
                        var nodePatternItems = await GetNodePatternItemsAsync(catalogStacks);

                        nodePatterns.Add(new()
                        {
                            NodeId = listNode.Id,
                            BunchId = listNode.BunchId,
                            Number = listNode.NodeExt.Number,
                            Items = nodePatternItems
                        });
                    }

                    NodePatternItem nodeItem = new(nodePatterns, stack.FieldId, stack.CreatorInfo, stack.DateCreated);
                    nodeItem.NodeId = stack.NodeId;
                    nodeItems.Add(nodeItem);
                }

                if (fieldType == (int)FieldType.File)
                {
                    await context.Entry(stack).Reference(x => x.StackFile).LoadAsync();
                    var fileData = new FileData()
                    {
                        Name = stack.StackFile.FileName,
                        Mime = stack.StackFile.FileType,
                        ByteArray = stack.StackFile.Data,
                        Size = stack.StackFile.FileSize
                    };
                    NodePatternItem nodeItem = new(fileData, stack.FieldId, stack.CreatorInfo, stack.DateCreated);
                    nodeItem.NodeId = stack.NodeId;
                    nodeItems.Add(nodeItem);
                }

            }

            return nodeItems;
        }
    }
}
