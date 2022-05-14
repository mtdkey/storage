using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public partial class RequestProvider : IDisposable
    {
        public async Task<RequestResult<FieldPattern>> FieldQueryAsync(Action<RequestFilter> filter)
        {
            RequestFilter requestFilter = new();
            filter.Invoke(requestFilter);
            return await FieldQueryAsync(requestFilter);
        }

        public async Task<RequestResult<FieldPattern>> FieldQueryAsync(RequestFilter filter)
        {
            var requestResult = new RequestResult<FieldPattern>(true);
  
            try
            {
                if (string.IsNullOrEmpty(filter.BunchName) is not true)
                {
                    var schema = await GetScheamaAsync(filter.BunchName);
                    var banchId = schema.DataSet.First().BunchPattern.BunchId;
                    filter.BunchIds.Add(banchId);
                }

                var query = context.Set<Field>()
                    .Where(field => field.DeletedFlag == FlagSign.False)
                    .FilterBasic(filter)
                    .FilterChild(filter);


                if (string.IsNullOrEmpty(filter.SearchText) is not true)
                {
                    var text = filter.SearchText.ToUpper();
                    query = query.Where(x => x.Name.ToUpper().Contains(text));
                }

                var dataSet = await query                    
                    .Select(field => new FieldPattern
                    {
                        FieldId = field.Id,
                        BunchId = field.BunchId,
                        Name = field.Name,
                        FieldType = field.FieldType,
                    })
                    .FilterPages(filter.Page, filter.PageSize)
                    .ToListAsync();

                requestResult.FillDataSet(dataSet);
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
