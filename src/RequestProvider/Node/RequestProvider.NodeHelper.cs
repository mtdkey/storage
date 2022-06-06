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

                if (fieldType == FieldType.Numeric)
                {
                    await context.Entry(stack).Reference(x => x.StackDigital).LoadAsync();
                    var value = stack.StackDigital.Value;
                    NodePatternItem nodeItem = new(value, stack.FieldId, stack.CreatorInfo, stack.DateCreated);
                    nodeItem.NodeId = stack.NodeId;
                    nodeItems.Add(nodeItem);
                }

                if (fieldType == FieldType.Boolean)
                {
                    await context.Entry(stack).Reference(x => x.StackDigital).LoadAsync();
                    bool value = stack.StackDigital.Value == 1;
                    NodePatternItem nodeItem = new(value, stack.FieldId, stack.CreatorInfo, stack.DateCreated);
                    nodeItem.NodeId = stack.NodeId;
                    nodeItems.Add(nodeItem);
                }

                if (fieldType == FieldType.DateTime)
                {
                    await context.Entry(stack).Reference(x => x.StackDigital).LoadAsync();
                    DateTime value = new((long)stack.StackDigital.Value);
                    NodePatternItem nodeItem = new(value, stack.FieldId, stack.CreatorInfo, stack.DateCreated);
                    nodeItem.NodeId = stack.NodeId;
                    nodeItems.Add(nodeItem);
                }

                if (fieldType == FieldType.Text)
                {
                    await context.Entry(stack).Collection(x => x.StackTexts).LoadAsync();
                    List<string> values = stack.StackTexts.Select(x => x.Value).ToList();
                    string value = string.Concat(values);
                    NodePatternItem nodeItem = new(value, stack.FieldId, stack.CreatorInfo, stack.DateCreated);
                    nodeItem.NodeId = stack.NodeId;
                    nodeItems.Add(nodeItem);
                }

                if (fieldType == FieldType.LinkSingle)
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

                if (fieldType == FieldType.File)
                {
                    await context.Entry(stack).Collection(x => x.StackFiles).LoadAsync();
                    var fileDatas = new List<FileData>();
                    foreach(var stackFile in stack.StackFiles)
                    {
                       fileDatas.Add(new()
                        {
                            StackId = stackFile.Id,
                            Name = stackFile.FileName,
                            Mime = stackFile.FileType,
                            ByteArray = stackFile.Data,
                            Size = stackFile.FileSize
                        });
                    }

                    NodePatternItem nodeItem = new(fileDatas, stack.FieldId, stack.CreatorInfo, stack.DateCreated);
                    nodeItem.NodeId = stack.NodeId;
                    nodeItems.Add(nodeItem);
                }

            }

            return nodeItems;
        }
    }
}
