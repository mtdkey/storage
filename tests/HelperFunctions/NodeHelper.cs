using MtdKey.Storage.Tests.TestFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage.Tests.HelperFunctions
{
    public static class NodeHelper
    {

        public static async Task<RequestResult<NodeSchema>> CreateAsync(RequestProvider requestProvider, BunchSchema bunchSchema = default)
        {
            var requestResult = new RequestResult<NodeSchema>(true);

            var bunch = bunchSchema ?? await requestProvider.CreateBunchAsync();
            var fieldLongText = await requestProvider.CreateFieldAsync(bunch.BunchId, FieldType.Text);            
            var fieldBoolean = await requestProvider.CreateFieldAsync(bunch.BunchId, FieldType.Boolean);
            var fieldDateTime = await requestProvider.CreateFieldAsync(bunch.BunchId, FieldType.DateTime);
            var fieldNumeric = await requestProvider.CreateFieldAsync(bunch.BunchId, FieldType.Numeric);

            List<NodeSchemaItem> nodeItems = new()
            {
                new NodeSchemaItem(Common.LongTextValue, fieldLongText.FieldId, "Tester", DateTime.UtcNow),   
                new NodeSchemaItem(Common.DateTimeValue, fieldDateTime.FieldId, "Tester", DateTime.UtcNow),
                new NodeSchemaItem(Common.BooleanValue, fieldBoolean.FieldId, "Tester", DateTime.UtcNow),
                new NodeSchemaItem(Common.NumericValue, fieldNumeric.FieldId, "Tester", DateTime.UtcNow),
            };          

            requestResult = await requestProvider.NodeSaveAsync(schema => {
                schema.BunchId = bunch.BunchId;
                schema.Items = nodeItems;
                schema.DateCreated = DateTime.UtcNow;
                schema.CreatorInfo = "Tester";
            });            

            return requestResult;
        }
    }
}
