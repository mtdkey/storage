using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.Storage.Tests.HelperFunctions
{
    public static class BunchHelper
    {
        public static async Task<BunchPattern> CreateBunchAsync(this RequestProvider requestProvider)
        {
            var createdBunch = await CreateAsync(requestProvider);
            return createdBunch.DataSet.FirstOrDefault();
        }

        public static async Task<RequestResult<BunchPattern>> CreateAsync(RequestProvider requestProvider)
        {
            string name = Common.GetRandomName();

            return await requestProvider.BunchSaveAsync(bunch => {
                bunch.Name = $"Bunch name is {name}";  
            });
        }

        public static async Task<RequestResult<BunchPattern>> CreateArchiveAsync(RequestProvider requestProvider)
        {
            string name = Common.GetRandomName();

            return await requestProvider.BunchSaveAsync(bunch => {
                bunch.Name = $"Bunch name is {name}";
            });
        }

    }
}
