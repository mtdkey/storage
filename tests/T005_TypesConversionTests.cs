using System.Threading.Tasks;
using Xunit;

namespace MtdKey.Storage.Tests
{
    public class T005_TypesConversionTests
    {
        [Fact]
        public void FieldTypeTests()
        {
            var fieldPattern = new FieldPattern { FieldType = FieldType.Text };
            Assert.True(FieldType.Text == fieldPattern.FieldType);
            Assert.True(fieldPattern.FieldType.Equals(FieldType.Text));

            Assert.True(FieldType.Text == 2);
            Assert.True(fieldPattern.FieldType == 2);
            Assert.True(fieldPattern.FieldType.Equals(2));
        }

        [Fact]
        public void LinkTypeTests()
        {
            var fieldPattern = new FieldPattern { LinkType = LinkType.Multiple };
            Assert.True(LinkType.Multiple == fieldPattern.LinkType);
            Assert.True(fieldPattern.LinkType.Equals(LinkType.Multiple));

            Assert.True(2 == LinkType.Multiple);
            Assert.True(fieldPattern.LinkType == 2);
            Assert.True(fieldPattern.LinkType.Equals(2));
        }
    }
}
