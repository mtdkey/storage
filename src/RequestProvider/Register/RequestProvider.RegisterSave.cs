using MtdKey.Storage.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MtdKey.Storage
{
    public partial class RequestProvider : IDisposable
    {
        public async Task<RequestResult<RegisterSchema>> RegisterSaveAsync(Action<RegisterSchema> registerSchema)
        {
            var schema = new RegisterSchema();
            registerSchema.Invoke(schema);

            return schema.RegisterId.Equals(Guid.Empty) ? await RegisterCreateAsync(schema) : await RegisterUpdateAsync(schema);
        }

        private async Task<RequestResult<RegisterSchema>> RegisterCreateAsync(RegisterSchema schema)
        {
            var requestResult = new RequestResult<RegisterSchema>(true);
            Register register = new()
            {
                Id = Guid.NewGuid(),
                Name = schema.Name,
                Value = schema.Value
            };

            try
            {
                await context.AddAsync(register);
                await context.SaveChangesAsync();

                schema.RegisterId = register.Id;
                requestResult.FillDataSet(new List<RegisterSchema>() { schema });
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

        private async Task<RequestResult<RegisterSchema>> RegisterUpdateAsync(RegisterSchema schema)
        {
            var requestResult = new RequestResult<RegisterSchema>(true);

            Register register = await context.FindAsync<Register>(schema.RegisterId);
            if (register == null)
            {
                requestResult.SetResultInfo(false, new Exception("Bad Request.")); return requestResult;
            }

            register.Name = schema.Name;
            register.Value = schema.Value;

            try
            {
                await context.SaveChangesAsync();
                requestResult.FillDataSet(new List<RegisterSchema>() { schema });
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
