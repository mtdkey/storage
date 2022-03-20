using MtdKey.Storage.DataModels;
using System;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public partial class RequestProvider : IDisposable
    {
        public async Task<IRequestResult> NodeDeleteAsync(long id)
        {
            var requestResult = new RequestResult<bool>(true);
           
            try
            {
                Node node = await context.FindAsync<Node>(id);

                ///Check access via token
                BunchToken bunchToken = await context.Set<BunchToken>().FindAsync(node.ParentId);
                bool deletingAllowed = contextProperty.AccessTokens.Contains(bunchToken.TokenoDelete);     

                if (deletingAllowed is not true)
                {
                    requestResult.SetResultInfo(false, new Exception("Access denied!"));
                    return requestResult;
                }

                node.DeletedFlag = FlagSign.True;
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
