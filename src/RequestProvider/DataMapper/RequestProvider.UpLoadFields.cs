using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MtdKey.Storage.DataModels;

namespace MtdKey.Storage
{
    public partial class RequestProvider : IDisposable
    {
        public async Task<IRequestResult> UpLoadFields(List<FieldTag> fieldTags)
        {
            try
            {
                await ParsingFieldTags(fieldTags);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new RequestResult<IRequestResult>(false, ex);
            }

            return new RequestResult<IRequestResult>(true);
        }

        private async Task ParsingFieldTags(List<FieldTag> fieldTags)
        {
            var bunchQuery = context.Set<Bunch>();
            var fieldQuery = context.Set<Field>();            

            foreach (var fieldTag in fieldTags)
            {
                long bunchId = bunchQuery.FirstOrDefault(x => x.Name.Equals(fieldTag.BunchName)).Id;

                var fieldExists = fieldQuery
                    .Where(field => field.Name.ToLower() == fieldTag.FieldSchema.Name.ToLower() 
                        && field.BunchId == bunchId)
                    .Any();
                if (fieldExists) continue;

                var field = new Field
                {
                    BunchId = bunchId,
                    Name = fieldTag.FieldSchema.Name,
                    FieldType = (int)fieldTag.FieldSchema.FieldType,                    
                };

                if (fieldTag.FieldSchema.FieldType == FieldType.Link)
                {
                    var linkId = bunchQuery.FirstOrDefault(x => x.Name.Equals(fieldTag.ListBunch)).Id;
                    var fieldLink = new FieldLink{ BunchId = linkId };
                    field.FieldLink = fieldLink;
                }

                await context.AddAsync(field);
            }
        }
    }
}
