using MtdKey.Storage.DataModels;
using System;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public partial class RequestProvider : IDisposable
    {

        public async Task<IRequestResult> FieldDeleteAsync(long id)
        {
            var requestResult = new RequestResult<FieldSchema>(true);
            Field field = await context.FindAsync<Field>(id);

            try
            {
                field.DeletedFlag = FlagSign.True;
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
