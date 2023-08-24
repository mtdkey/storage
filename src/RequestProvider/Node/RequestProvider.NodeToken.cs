using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public partial class RequestProvider : IDisposable
    {

        public async Task<IRequestResult> NodeChangeTokenForRLS(string oldToken, string newToken, long nodeId = default)
        {
            var requestResult = new RequestResult<bool>(true);
            try
            {
                var query = context.Set<NodeToken>().AsQueryable();
                if (nodeId > 0) { query = query.Where(x => x.NodeId == nodeId); }

                IList<NodeToken> nodeTokens = await query.Where(node => node.ForRLS == oldToken).ToListAsync();
                foreach (var nodeToken in nodeTokens)
                {
                    nodeToken.ForRLS = newToken;
                }

                await context.SaveChangesAsync();

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
    }
}
