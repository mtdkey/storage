using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public partial class RequestProvider : IDisposable
    {

        public async Task<RequestResult<BunchSchema>> BunchQueryAsync(Action<RequestFilter> filter)
        {
            RequestFilter requestFilter = new();
            filter.Invoke(requestFilter);
            return await BunchQueryAsync(requestFilter); 
        }

        public async Task<RequestResult<BunchSchema>> BunchQueryAsync(RequestFilter filter)
        {
            var schemaResult = new RequestResult<BunchSchema>(true);
            
            try
            {                
                var query = context.Set<Bunch>()
                    .FilterBasic(filter)
                    .FilterPages(filter.Page, filter.PageSize);

                if (string.IsNullOrEmpty(filter.SearchText) is not true)
                {
                    var text = filter.SearchText.ToUpper();
                    query = query.Where(bunch => bunch.Name.ToUpper().Contains(text) || bunch.Description.ToUpper().Contains(text));
                }

                var dataSet = await query                    
                    .Select(bunch => new BunchSchema
                    {
                        BunchId = bunch.Id,
                        Name = bunch.Name,
                        Description = bunch.Description,
                        ArchiveFlag = bunch.ArchiveFlag.AsBoolean()

                    }).ToListAsync();

                schemaResult.FillDataSet(dataSet);
            }
            catch (Exception exception)
            {
                schemaResult.SetResultInfo(false, exception);
#if DEBUG
                throw;
#endif
            }

            return schemaResult;
        }


    }
}
