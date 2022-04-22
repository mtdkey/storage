using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MtdKey.Storage.DataModels;

namespace MtdKey.Storage
{
    public partial class RequestProvider : IDisposable
    {
        public async Task<IRequestResult> UpLoadSchena(List<BunchPattern> bunchTags)
        {
            try
            {
                await ParsingTBunchags(bunchTags);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new RequestResult<IRequestResult>(false, ex);
            }

            return new RequestResult<IRequestResult>(true);
        }

        private async Task ParsingTBunchags(List<BunchPattern> bunchTags)
        {
            var query = context.Set<Bunch>();

            foreach (var bunchTag in bunchTags)
            {
                var bunchExists = query
                    .Where(bunch => bunch.Name.ToLower() == bunchTag.Name.ToLower())
                    .Any();
                if (bunchExists) continue;

                var bunch = new Bunch { Name = bunchTag.Name };
                await context.AddAsync(bunch);
            }
        }


        public async Task<IRequestResult> UpLoadSchena(List<FieldTag> fieldTags)
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
                    .Where(field => field.Name.ToLower() == fieldTag.FieldPattern.Name.ToLower()
                        && field.BunchId == bunchId)
                    .Any();
                if (fieldExists) continue;

                var field = new Field
                {
                    BunchId = bunchId,
                    Name = fieldTag.FieldPattern.Name,
                    FieldType = (int)fieldTag.FieldPattern.FieldType,
                };

                if (fieldTag.FieldPattern.FieldType == FieldType.Link)
                {
                    var linkId = bunchQuery.FirstOrDefault(x => x.Name.Equals(fieldTag.BunchList)).Id;
                    var fieldLink = new FieldLink { BunchId = linkId };
                    field.FieldLink = fieldLink;
                }

                await context.AddAsync(field);
            }
        }

    }
}
