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
        public async Task<RequestResult<NodePattern>> NodeQueryAsync(Action<RequestFilter> filter)
        {
            var patternResult = new RequestResult<NodePattern>(true);
            var requestFilter = new RequestFilter();
            filter.Invoke(requestFilter);

            try
            {
                if (requestFilter.BunchNames.Count > 0)
                {
                    foreach (var bunchName in requestFilter.BunchNames)
                    {
                        var bunchFields = await BunchQueryAsync(filter => filter.BunchNames.Add(bunchName));
                        var banchId = bunchFields.DataSet.First().BunchId;
                        requestFilter.BunchIds.Add(banchId);
                    }
                }

                var query = context.Set<Node>()
                    .Where(node => node.DeletedFlag == FlagSign.False
                        && contextProperty.AccessTokens.Contains(node.NodeToken.ForRLS));

                if (requestFilter.NodeIds?.Count > 0)
                    query = query.Where(node => requestFilter.NodeIds.Contains(node.Id));

                if (requestFilter.BunchIds?.Count > 0)
                    query = query.Where(node => requestFilter.BunchIds.Contains(node.BunchId));                   

                if (string.IsNullOrEmpty(requestFilter.SearchText) is not true)
                {
                    var text = requestFilter.SearchText.ToUpper();
                    var script = SqlScript.SearchText(text, contextProperty.DatabaseType);
                    var scriptForLinks = SqlScript.SearchTextByLink(text, contextProperty.DatabaseType);
                    IList<long> textNodeids = await context.Set<Node>().FromSqlRaw(script).Select(x => x.Id).ToListAsync();
                    IList<long> linkNodeids = await context.Set<Node>().FromSqlRaw(scriptForLinks).Select(x => x.Id).ToListAsync();
                    var ids = textNodeids.Union(linkNodeids).Distinct().ToList();
                    query = query.Where(x => ids.Contains(x.Id));
                }

                var scriptGetMaxIds = SqlScript.StackMaxIds(contextProperty.DatabaseType);                
                var stackQuery = context.Set<Stack>().FromSqlRaw(scriptGetMaxIds);
                
                if (requestFilter.FieldIds?.Count > 0)
                    stackQuery = stackQuery.Where(stack => requestFilter.FieldIds.Contains(stack.FieldId));
                
                var nodeIds = await stackQuery.GroupBy(x=>x.NodeId).Select(x=>x.Key).ToListAsync();
                patternResult.SetRowCount(nodeIds.Count);

                IList<NodePattern> dataSet = await query.Where(x=>nodeIds.Contains(x.Id))
                    .Select(node => new NodePattern
                    {
                        NodeId = node.Id,
                        BunchId = node.BunchId,
                        Number = node.NodeExt.Number,
                        DateCreated = node.DateCreated,
                        CreatorInfo = node.CreatorInfo,
                        Items = new List<NodePatternItem>()
                    })
                    .OrderByDescending(x => x.DateCreated)
                    .FilterPages(requestFilter.Page, requestFilter.PageSize)
                    .ToListAsync();

                nodeIds = dataSet.GroupBy(x => x.NodeId).Select(x => x.Key).ToList();
                stackQuery = stackQuery.Where(x => nodeIds.Contains(x.NodeId));

                IList<Stack> stacks = await stackQuery.ToListAsync();
                List<NodePatternItem> nodeItems = await GetNodePatternItemsAsync(stacks);

                dataSet.ToList().ForEach(node =>
                {
                    node.Items = nodeItems.Where(x => x.NodeId == node.NodeId).ToList();
                });
                
                patternResult.FillDataSet(dataSet as List<NodePattern>);                
            }
            catch (Exception exception)
            {
                patternResult.SetResultInfo(false, exception);
#if DEBUG
                throw;
#endif
            }

            return patternResult;
        }
    }
}
