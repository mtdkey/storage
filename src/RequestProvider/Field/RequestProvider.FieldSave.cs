using MtdKey.Storage.DataModels;
using System;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public partial class RequestProvider : IDisposable
    {
        public async Task<RequestResult<FieldSchema>> FieldSaveAsync(Action<FieldSchema> fieldSchema)
        {
            var schema = new FieldSchema();
            fieldSchema.Invoke(schema);
            return schema.FieldId > 0 ? await FieldUpdatedAsync(schema) : await FieldCreateAsync(schema);
        }

        public async Task<RequestResult<FieldSchema>> FieldSaveAsync(FieldSchema fieldSchema)
        {
            return fieldSchema.FieldId>0 ? await FieldUpdatedAsync(fieldSchema) : await FieldCreateAsync(fieldSchema);
        }

        private async Task<RequestResult<FieldSchema>> FieldCreateAsync(FieldSchema fieldSchema)
        {
            var requestResult = new RequestResult<FieldSchema>(true);

            var field = new Field
            {
                BunchId = fieldSchema.BunchId,
                Name = fieldSchema.Name ?? string.Empty,
                Description = fieldSchema.Description ?? string.Empty,
                FieldType = (int)fieldSchema.FieldType,
                ArchiveFlag = fieldSchema.ArchiveFlag.AsFlagSign(),
                DeletedFlag = FlagSign.False,
            };

            if (fieldSchema.FieldType == FieldType.Link)
            {
                var fieldLink = new FieldLink
                {
                    BunchId = fieldSchema.LinkId
                };
                field.FieldLink = fieldLink;
            }

            try
            {
                await context.AddAsync(field);
                await context.SaveChangesAsync();
                
                fieldSchema.FieldId = field.Id;
                requestResult.FillDataSet(new() { fieldSchema });
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

        private async Task<RequestResult<FieldSchema>> FieldUpdatedAsync(FieldSchema fieldSchema)
        {
            var requestResult = new RequestResult<FieldSchema>(true);
            Field field = await context.FindAsync<Field>(fieldSchema.FieldId);
            if (field == null || field.DeletedFlag == FlagSign.True) { requestResult.SetResultInfo(false, new Exception("Bad Request.")); return requestResult; }
            
            field.Name = fieldSchema.Name ?? field.Name;
            field.Description = fieldSchema.Description ?? field.Description;
            field.ArchiveFlag = fieldSchema.ArchiveFlag.AsFlagSign();
            field.DeletedFlag = FlagSign.False;

            try
            {
                await context.SaveChangesAsync();
                requestResult.FillDataSet(new() { fieldSchema });
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
