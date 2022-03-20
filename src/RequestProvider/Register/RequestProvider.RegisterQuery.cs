using Microsoft.EntityFrameworkCore;
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
        public async Task<RequestResult<RegisterSchema>> RegisterQueryAsync(Action<RequestFilter> filter)
        {
            var requestFilter = new RequestFilter();
            filter.Invoke(requestFilter);
            
            var requestResult = new RequestResult<RegisterSchema>(true);

//            try
//            {
//                var query = context.Set<Register>()
//                    .FilterBasic(requestFilter)
//                    .FilterChild(requestFilter)
//                    .FilterPages(requestFilter.Page, requestFilter.PageSize);

//                if (string.IsNullOrEmpty(filter.SearchText) is not true)
//                {
//                    var text = filter.SearchText.ToUpper();
//                    query = query.Where(x => x.Name.ToUpper().Contains(text) || x.Description.ToUpper().Contains(text));
//                }

//                var dataSet = await query
//                    .Select(field => new FieldSchema
//                    {
//                        FieldId = field.Id,
//                        BunchId = field.ParentId,
//                        Name = field.Name,
//                        FieldType = field.FieldType,
//                        Description = field.Description,
//                        ArchiveFlag = field.ArchiveFlag.AsBoolean()
//                    }).ToListAsync();

//                requestResult.FillDataSet(dataSet);
//            }
//            catch (Exception exception)
//            {
//                requestResult.SetResultInfo(false, exception);
//#if DEBUG
//                throw;
//#endif
//            }

            return requestResult;

        }
    }
}
