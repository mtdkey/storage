using MtdKey.Storage.DataModels;
using System;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public partial class RequestProvider : IDisposable
    {
        public async Task<RequestResult<BunchPattern>> BunchSaveAsync(BunchPattern bunchPattern)
        {
            return bunchPattern.BunchId > 0 ? await BunchUpdateAsync(bunchPattern) : await BunchCreateAsync(bunchPattern);
        }

        public async Task<RequestResult<BunchPattern>> BunchSaveAsync(Action<BunchPattern> bunchPattern)
        {
            var pattern = new BunchPattern();
            bunchPattern.Invoke(pattern);
            return await BunchSaveAsync(pattern);
        }

        private async Task<RequestResult<BunchPattern>> BunchCreateAsync(BunchPattern bunchPattern)
        {
            var requestResult = new RequestResult<BunchPattern>(true);

            var bunch = new Bunch
            {
                Name = bunchPattern.Name ?? string.Empty,             
                DeletedFlag = FlagSign.False,
                BunchExt = new BunchExt(),
                BunchToken = new BunchToken()
                {
                    TokenoDelete = string.Empty,
                    TokenToCreate =string.Empty,
                    TokenToEdit = string.Empty
                }
            };

            try
            {
                await context.AddAsync(bunch);
                await context.SaveChangesAsync();

                bunchPattern.BunchId = bunch.Id;
                requestResult.FillDataSet(new() { bunchPattern });         
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

        private async Task<RequestResult<BunchPattern>> BunchUpdateAsync(BunchPattern bunchPattern)
        {
            var requestResult = new RequestResult<BunchPattern>(true);
            Bunch bunch = await context.FindAsync<Bunch>(bunchPattern.BunchId);
            if (bunch == null || bunch.DeletedFlag == FlagSign.True) { requestResult.SetResultInfo(false, new Exception("Bad Request.")); return requestResult; }

            bunch.Name = bunchPattern.Name ?? bunch.Name;
            bunch.DeletedFlag = FlagSign.False;

            try
            {
                await context.SaveChangesAsync();
                requestResult.FillDataSet(new() { bunchPattern });
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
