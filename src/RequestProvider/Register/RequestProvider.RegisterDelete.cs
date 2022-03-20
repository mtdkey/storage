using MtdKey.Storage.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public partial class RequestProvider : IDisposable
    {
        public async Task<IRequestResult> RegisterDeleteAsync(Guid id)
        {
            var requestResult = new RequestResult<RegisterSchema>(true);
            var register = new Register() { Id = id };

            try
            {
                context.Remove(register);
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
