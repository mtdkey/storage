using MtdKey.Storage.DataModels;
using System;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public partial class RequestProvider : IDisposable
    {
        public async Task<RequestResult<BunchSchema>> BunchSaveAsync(BunchSchema bunchSchema)
        {
            return bunchSchema.BunchId > 0 ? await BunchUpdateAsync(bunchSchema) : await BunchCreateAsync(bunchSchema);
        }

        public async Task<RequestResult<BunchSchema>> BunchSaveAsync(Action<BunchSchema> bunchSchema)
        {
            var schema = new BunchSchema();
            bunchSchema.Invoke(schema);
            return await BunchSaveAsync(schema);
        }

        private async Task<RequestResult<BunchSchema>> BunchCreateAsync(BunchSchema bunchSchema)
        {
            var requestResult = new RequestResult<BunchSchema>(true);

            var bunch = new Bunch
            {
                Name = bunchSchema.Name ?? string.Empty,
                Description = bunchSchema.Description ?? string.Empty,
                ArchiveFlag = bunchSchema.ArchiveFlag.AsFlagSign(),                
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

                bunchSchema.BunchId = bunch.Id;
                requestResult.FillDataSet(new() { bunchSchema });         

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

        private async Task<RequestResult<BunchSchema>> BunchUpdateAsync(BunchSchema bunchSchema)
        {
            var requestResult = new RequestResult<BunchSchema>(true);
            Bunch bunch = await context.FindAsync<Bunch>(bunchSchema.BunchId);
            if (bunch == null || bunch.DeletedFlag == FlagSign.True) { requestResult.SetResultInfo(false, new Exception("Bad Request.")); return requestResult; }

            bunch.Name = bunchSchema.Name ?? bunch.Name;
            bunch.Description = bunchSchema.Description ?? bunch.Description;
            bunch.ArchiveFlag = bunchSchema.ArchiveFlag.AsFlagSign();
            bunch.DeletedFlag = FlagSign.False;

            try
            {
                await context.SaveChangesAsync();
                requestResult.FillDataSet(new() { bunchSchema });
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
