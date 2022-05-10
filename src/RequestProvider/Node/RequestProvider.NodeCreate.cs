using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;
using MtdKey.Storage.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public partial class RequestProvider : IDisposable
    {

        public async Task<RequestResult<NodePattern>> NodeCreateAsync(string bunchName, string fieldName, object value, string creatorInfo)
        {
            return await NodeCreateAsync(bunchName, new() { { fieldName, value } }, creatorInfo);
        }

        public async Task<RequestResult<NodePattern>> NodeCreateAsync(string bunchName, Dictionary<string, object> values, string creatorInfo)
        {
            return await ConvertToNodePattern(bunchName, values, creatorInfo, true);
        }

        public async Task<RequestResult<NodePattern>> NodeUpdateAsync(string bunchName, Dictionary<string, object> values, string creatorInfo)
        {
           return await ConvertToNodePattern(bunchName,values, creatorInfo);
        }

        private async Task<RequestResult<NodePattern>> ConvertToNodePattern(string bunchName, Dictionary<string, object> values, string creatorInfo, bool createNew = false)
        {
            var schemaRetrived = await GetScheamaAsync(bunchName);
            var bunchId = schemaRetrived.DataSet.First().BunchPattern.BunchId;

            var nodePattern = new NodePattern()
            {
                BunchId = bunchId,
                CreatorInfo = creatorInfo,
                DateCreated = DateTime.UtcNow,
                Items = new List<NodePatternItem>(),
            };

            foreach (var pair in values)
            {
                var filedPaterns = schemaRetrived.DataSet.FirstOrDefault()?.FieldPatterns;
                if (filedPaterns == null) continue;
                var field = filedPaterns.FirstOrDefault(x => x.Name == pair.Key);
                if (field == null) continue;

                object value = pair.Value;
                if (field.FieldType.IsLink)
                {
                    var ok = long.TryParse((string)pair.Value, out long nodeId);
                    if (!ok) return new(false, new Exception("NodeId parsing error!"));

                    var request = await NodeQueryAsync(filter => filter.Ids.Add(nodeId));
                    if (!request.Success) return request;
                    value = request.DataSet;
                }

                nodePattern.Items.Add(new(value, field.FieldId, field.FieldType, creatorInfo, DateTime.UtcNow));
            }


            if (createNew)
            {
                nodePattern.NodeId = 0;
            }
            else
            {
                if(!long.TryParse((string)values["Id"], out long id))
                {
                    return new RequestResult<NodePattern>(false, new Exception("Node ID is wrong!"));
                }

                nodePattern.NodeId = id;
            }
            
            return await NodeSaveAsync(nodePattern);
        }

    }
}
