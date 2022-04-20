using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public partial class RequestProvider : IDisposable
    {
        public async Task<RequestResult<FieldSchema>> FieldQueryAsync(Action<RequestFilter> filter)
        {
            RequestFilter requestFilter = new();
            filter.Invoke(requestFilter);
            return await FieldQueryAsync(requestFilter);
        }

        public async Task<RequestResult<FieldSchema>> FieldQueryAsync(RequestFilter filter)
        {
            var requestResult = new RequestResult<FieldSchema>(true);
  
            try
            {
                var query = context.Set<Field>()
                    .FilterBasic(filter)
                    .FilterChild(filter)
                    .FilterPages(filter.Page, filter.PageSize);

                if (string.IsNullOrEmpty(filter.SearchText) is not true)
                {
                    var text = filter.SearchText.ToUpper();
                    query = query.Where(x => x.Name.ToUpper().Contains(text) || x.Description.ToUpper().Contains(text));
                }

                var dataSet = await query                    
                    .Select(field => new FieldSchema
                    {
                        FieldId = field.Id,
                        BunchId = field.BunchId,
                        Name = field.Name,
                        FieldType = field.FieldType,
                        Description = field.Description,                        
                        ArchiveFlag = field.ArchiveFlag.AsBoolean()
                    }).ToListAsync();

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
