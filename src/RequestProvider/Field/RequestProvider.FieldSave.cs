using MtdKey.Storage.DataModels;
using System;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public partial class RequestProvider : IDisposable
    {
        public async Task<RequestResult<FieldPattern>> FieldSaveAsync(Action<FieldPattern> FieldPattern)
        {
            var pattern = new FieldPattern();
            FieldPattern.Invoke(pattern);
            return pattern.FieldId > 0 ? await FieldUpdatedAsync(pattern) : await FieldCreateAsync(pattern);
        }

        public async Task<RequestResult<FieldPattern>> FieldSaveAsync(FieldPattern FieldPattern)
        {
            return FieldPattern.FieldId > 0 ? await FieldUpdatedAsync(FieldPattern) : await FieldCreateAsync(FieldPattern);
        }

        private async Task<RequestResult<FieldPattern>> FieldCreateAsync(FieldPattern FieldPattern)
        {
            var requestResult = new RequestResult<FieldPattern>(true);

            var field = new Field
            {
                BunchId = FieldPattern.BunchId,
                Name = FieldPattern.Name ?? string.Empty,                
                FieldType = (int)FieldPattern.FieldType,
                DeletedFlag = FlagSign.False,
            };

            if (FieldPattern.FieldType == FieldType.Link)
            {
                var fieldLink = new FieldLink
                {
                    BunchId = FieldPattern.LinkId
                };
                field.FieldLink = fieldLink;
            }

            try
            {
                await context.AddAsync(field);
                await context.SaveChangesAsync();

                FieldPattern.FieldId = field.Id;
                requestResult.FillDataSet(new() { FieldPattern });
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

        private async Task<RequestResult<FieldPattern>> FieldUpdatedAsync(FieldPattern FieldPattern)
        {
            var requestResult = new RequestResult<FieldPattern>(true);
            Field field = await context.FindAsync<Field>(FieldPattern.FieldId);
            if (field == null || field.DeletedFlag == FlagSign.True)
            {
                requestResult.SetResultInfo(false, new Exception("Bad Request."));
                return requestResult;
            }

            field.Name = FieldPattern.Name ?? field.Name;
            field.DeletedFlag = FlagSign.False;

            try
            {
                await context.SaveChangesAsync();
                requestResult.FillDataSet(new() { FieldPattern });
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
