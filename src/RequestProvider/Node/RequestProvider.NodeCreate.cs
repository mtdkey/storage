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
            var categoryRetrived = await GetScheamaAsync(bunchName);
            var bunchId = categoryRetrived.DataSet.First().BunchPattern.BunchId;

            var nodePattern = new NodePattern()
            {
                BunchId = bunchId,
                CreatorInfo = creatorInfo,
                DateCreated = DateTime.UtcNow,
                Items = new List<NodePatternItem>(),
            };

            foreach (var pair in values)
            {
                var field = categoryRetrived.DataSet.First().FieldPatterns
                    .Where(x => x.Name == pair.Key).First();

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

            nodePattern.NodeId = 0;
            return await NodeSaveAsync(nodePattern);
        }

    }
}
