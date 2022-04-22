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
                var query = context.Set<Node>()
                    .Where(node => node.DeletedFlag == FlagSign.False
                        && contextProperty.AccessTokens.Contains(node.NodeToken.ForRLS))
                    .FilterBasic(requestFilter)
                    .FilterChild(requestFilter);                

                if (string.IsNullOrEmpty(requestFilter.SearchText) is not true)
                {
                    var text = requestFilter.SearchText.ToUpper();
                    var script = SqlScript.SearchText(text, contextProperty.DatabaseType);                                                                      
                    IList<long> textNodeids = await context.Set<Node>().FromSqlRaw(script).Select(x => x.Id).ToListAsync();                   
                    query = textNodeids.Count > 0 ? query.Where(x => textNodeids.Contains(x.Id)) : query;
                }

                var dataSet = await query                    
                    .Select(node => new NodePattern
                    {
                        NodeId = node.Id,
                        BunchId = node.BunchId,
                        Number = node.NodeExt.Number,
                        Items = new List<NodePatternItem>()

                    })
                    .FilterPages(requestFilter.Page, requestFilter.PageSize)
                    .ToListAsync();

                var scriptGetMaxIds = SqlScript.StackMaxIds(contextProperty.DatabaseType);
                List<long> nodeIds = dataSet.GroupBy(x => x.NodeId).Select(x => x.Key).ToList();

                IList<Stack> stacks = await context.Set<Stack>()
                    .FromSqlRaw(scriptGetMaxIds)                    
                    .Where(x => nodeIds.Contains(x.NodeId))
                    .ToListAsync();

                List<NodePatternItem> nodeItems = await GetNodePatternItemsAsync(stacks);

                dataSet.ToList().ForEach(node =>
                {
                    node.Items = nodeItems.Where(x => x.NodeId == node.NodeId).ToList();
                });
                
                patternResult.FillDataSet(dataSet);
                
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
