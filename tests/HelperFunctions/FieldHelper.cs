using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.Storage.Tests.HelperFunctions
{
    public static class FieldHelper
    {
        public static async Task<FieldPattern> CreateFieldAsync(this RequestProvider requestProvider, long bunchId, FieldType fieldType, long linkId=0)
        {
            var createdField = await CreateAsync(requestProvider, bunchId, fieldType, linkId);
            return createdField.DataSet.FirstOrDefault();
        }

        public static async Task<RequestResult<FieldPattern>> CreateAsync(RequestProvider requestProvider, long bunchId, FieldType fieldType, long linkId = 0)
        {
            string name = Common.GetRandomName();

            return await requestProvider.FieldSaveAsync(field => {
                field.LinkId = linkId;
                field.BunchId = bunchId;
                field.FieldType = fieldType;
                field.Name = $"Field name is {name} {fieldType.GetXmlType}";
            });
        }

        public static async Task<RequestResult<FieldPattern>> CreateArchiveAsync(RequestProvider requestProvider, long bunchId, FieldType fieldType, long linkId = 0)
        {
            string name = Common.GetRandomName();

            return await requestProvider.FieldSaveAsync(field => {
                field.LinkId = linkId;
                field.BunchId = bunchId;
                field.FieldType = fieldType;
                field.Name = $"Field name is {name} {fieldType.GetXmlType}";
            });
        }

    }
}
