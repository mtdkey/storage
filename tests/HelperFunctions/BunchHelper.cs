using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.Storage.Tests.HelperFunctions
{
    public static class BunchHelper
    {
        public static async Task<BunchSchema> CreateBunchAsync(this RequestProvider requestProvider)
        {
            var createdBunch = await CreateAsync(requestProvider);
            return createdBunch.DataSet.FirstOrDefault();
        }

        public static async Task<RequestResult<BunchSchema>> CreateAsync(RequestProvider requestProvider)
        {
            string name = Common.GetRandomName();

            return await requestProvider.BunchSaveAsync(schema => {
                schema.Name = $"Bunch name is {name}";
                schema.Description = $"Bunch description is {name}";
            });
        }

        public static async Task<RequestResult<BunchSchema>> CreateArchiveAsync(RequestProvider requestProvider)
        {
            string name = Common.GetRandomName();

            return await requestProvider.BunchSaveAsync(schema => {
                schema.Name = $"Bunch name is {name}";
                schema.Description = $"Bunch description is {name}";
                schema.ArchiveFlag = true;
            });
        }

    }
}
