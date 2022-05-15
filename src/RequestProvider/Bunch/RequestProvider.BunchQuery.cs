using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public partial class RequestProvider : IDisposable
    {

        public async Task<RequestResult<BunchPattern>> BunchQueryAsync(Action<RequestFilter> filter)
        {
            RequestFilter requestFilter = new();
            filter.Invoke(requestFilter);
            return await BunchQueryAsync(requestFilter); 
        }

        public async Task<RequestResult<BunchPattern>> BunchQueryAsync(RequestFilter filter)
        {
            var patternResult = new RequestResult<BunchPattern>(true);            

            if(filter.FieldIds.Count>0 || filter.NodeIds.Count > 0)            
                return new RequestResult<BunchPattern>(false, 
                    new Exception("FieldIds or NodeIds are not supported for this query!"));            

            try
            {                
                if (filter.BunchNames.Count>0)
                {
                    foreach(var bunchName in filter.BunchNames)
                    {
                        var schema = await GetScheamaAsync(bunchName);
                        var banchId = schema.DataSet.First().BunchPattern.BunchId;
                        filter.BunchIds.Add(banchId);
                    }
                }

                var query = context.Set<Bunch>()
                    .Where(bunch => bunch.DeletedFlag == FlagSign.False);

                if (filter.BunchIds?.Count > 0)
                    query = query.Where(bunch => filter.BunchIds.Contains(bunch.Id));
                

                if (string.IsNullOrEmpty(filter.SearchText) is not true)
                {
                    var text = filter.SearchText.ToUpper();
                    query = query.Where(bunch => bunch.Name.ToUpper().Contains(text));
                }

                var dataSet = await query                    
                    .Select(bunch => new BunchPattern
                    {
                        BunchId = bunch.Id,
                        Name = bunch.Name,
                    })
                    .FilterPages(filter.Page, filter.PageSize)
                    .ToListAsync();

                patternResult.FillDataSet(dataSet);
            }
            catch (Exception exception)
            {
                patternResult.SetResultInfo(false, exception);
#if DEBUG
                throw;
#endif
            }

            return patternResult;
        }
    }
}
