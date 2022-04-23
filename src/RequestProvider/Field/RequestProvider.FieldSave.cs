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

        private async Task<RequestResult<FieldPattern>> FieldCreateAsync(FieldPattern fieldPattern)
        {
            var requestResult = new RequestResult<FieldPattern>(true);

            var field = new Field
            {
                BunchId = fieldPattern.BunchId,
                Name = fieldPattern.Name ?? string.Empty,                
                FieldType = (int)fieldPattern.FieldType,
                DeletedFlag = FlagSign.False,
            };

            if (fieldPattern.FieldType == FieldType.Link)
            {
                var fieldLink = new FieldLink
                {
                    BunchId = fieldPattern.LinkId,
                    LinkType = (int)fieldPattern.LinkType,
                };
                field.FieldLink = fieldLink;
            }

            try
            {
                await context.AddAsync(field);                
                await context.SaveChangesAsync();

                var newField = await context.Set<Field>().FindAsync(field.Id);
                await context.Entry(newField).Reference(x=>x.FieldLink).LoadAsync();

                fieldPattern.FieldId = newField.Id;
                fieldPattern.FieldType = newField.FieldType;
                fieldPattern.LinkType = newField.FieldLink?.LinkType ?? LinkType.Single;
                fieldPattern.BunchId = newField.BunchId;
                fieldPattern.Name = newField.Name;
                                
                requestResult.FillDataSet(new() { fieldPattern });
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
