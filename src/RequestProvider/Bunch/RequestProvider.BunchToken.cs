using MtdKey.Storage.DataModels;
using System;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public enum TokenAction { ToCreate, ToEdit, ToDelete }

    public partial class RequestProvider : IDisposable
    {

        public async Task<IRequestResult> BunchSetTokensAsync(TokenAction tokenAction, long bunchId, string token)
        {
            var requestResult = new RequestResult<bool>(true);

            try
            {
                var bunchToken = await context.Set<BunchToken>().FindAsync(bunchId);

                if (tokenAction.Equals(TokenAction.ToCreate))
                {
                    bunchToken.TokenToCreate = token;
                }

                if (tokenAction.Equals(TokenAction.ToEdit))
                {
                    bunchToken.TokenToEdit = token;
                }

                if (tokenAction.Equals(TokenAction.ToDelete))
                {
                    bunchToken.TokenoDelete = token;
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
