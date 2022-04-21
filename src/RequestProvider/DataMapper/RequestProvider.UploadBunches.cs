using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MtdKey.Storage.DataModels;

namespace MtdKey.Storage
{
    public partial class RequestProvider : IDisposable
    {
        public async Task<IRequestResult> UpLoadBunches(List<BunchSchema> bunchTags)
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

        private async Task ParsingTBunchags(List<BunchSchema> bunchTags)
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
    }
}
