using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.Storage.Tests.HelperFunctions
{
    public static class FieldHelper
    {
        public static async Task<FieldSchema> CreateFieldAsync(this RequestProvider requestProvider, long bunchId, FieldType fieldType)
        {
            var createdField = await CreateAsync(requestProvider, bunchId, fieldType);
            return createdField.DataSet.FirstOrDefault();
        }

        public static async Task<RequestResult<FieldSchema>> CreateAsync(RequestProvider requestProvider, long bunchId, FieldType fieldType)
        {
            string name = Common.GetRandomName();

            return await requestProvider.FieldSaveAsync(schema => {
                schema.BunchId = bunchId;
                schema.FieldType = fieldType;
                schema.Name = $"Field name is {name} {FieldType.GetName(fieldType)}";
                schema.Description = $"Field description is {name}";
            });
        }

        public static async Task<RequestResult<FieldSchema>> CreateArchiveAsync(RequestProvider requestProvider, long bunchId, FieldType fieldType)
        {
            string name = Common.GetRandomName();

            return await requestProvider.FieldSaveAsync(schema => {
                schema.BunchId = bunchId;
                schema.FieldType = fieldType;
                schema.Name = $"Field name is {name} {FieldType.GetName(fieldType)}";
                schema.Description = $"Field description is {name}";
                schema.ArchiveFlag = true;
            });
        }

    }
}
