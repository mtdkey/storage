using MtdKey.Storage.Tests.HelperFunctions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MtdKey.Storage.Tests
{
    [Collection("Sequential")]
    public class T050_DataMapper
    {
        [Fact]
        public void A_ReadDataFromFile()
        {                       
            var data = DataMapper.ReadDataFromFile();
            Assert.NotNull(data);
        }
    }
}
