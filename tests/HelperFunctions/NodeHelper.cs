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

        public static async Task<RequestResult<NodePattern>> CreateAsync(RequestProvider requestProvider, BunchPattern bunchPattern = default)
        {
            var requestResult = new RequestResult<NodePattern>(true);

            var bunch = bunchPattern ?? await requestProvider.CreateBunchAsync();
            var fieldLongText = await requestProvider.CreateFieldAsync(bunch.BunchId, FieldType.Text);            
            var fieldBoolean = await requestProvider.CreateFieldAsync(bunch.BunchId, FieldType.Boolean);
            var fieldDateTime = await requestProvider.CreateFieldAsync(bunch.BunchId, FieldType.DateTime);
            var fieldNumeric = await requestProvider.CreateFieldAsync(bunch.BunchId, FieldType.Numeric);
            var fieldFile = await requestProvider.CreateFieldAsync(bunch.BunchId, FieldType.File);

            var file = await Common.GetFileTestAsync();

            List<NodePatternItem> nodeItems = new()
            {
                new NodePatternItem(Common.LongTextValue, fieldLongText.FieldId, "Tester", DateTime.UtcNow),   
                new NodePatternItem(Common.DateTimeValue, fieldDateTime.FieldId, "Tester", DateTime.UtcNow),
                new NodePatternItem(Common.BooleanValue, fieldBoolean.FieldId, "Tester", DateTime.UtcNow),
                new NodePatternItem(Common.NumericValue, fieldNumeric.FieldId, "Tester", DateTime.UtcNow),
                new NodePatternItem(file, fieldFile.FieldId, "Tester", DateTime.UtcNow),
            };          

            requestResult = await requestProvider.NodeSaveAsync(node => {
                node.BunchId = bunch.BunchId;
                node.Items = nodeItems;
                node.DateCreated = DateTime.UtcNow;
                node.CreatorInfo = "Tester";
            });            

            return requestResult;
        }
    }
}
