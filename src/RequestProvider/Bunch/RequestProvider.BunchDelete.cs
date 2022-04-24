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

        public async Task<IRequestResult> BunchDeleteAsync(long id)
        {
            var requestResult = new RequestResult<BunchPattern>(true);

            try
            {
                Bunch bunch = await context.FindAsync<Bunch>(id);
                bunch.DeletedFlag = FlagSign.True;

                IList<Field> childFields = await context.Set<Field>().Where(x => x.BunchId == id).ToListAsync();
                foreach (var childField in childFields)
                {
                    childField.DeletedFlag = FlagSign.True;
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
