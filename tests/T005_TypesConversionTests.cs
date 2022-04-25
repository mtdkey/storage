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

            Assert.True(2 == FieldType.Text);
            Assert.True(fieldPattern.FieldType == 2);
            Assert.True(fieldPattern.FieldType.Equals(2));

            
            fieldPattern.FieldType = FieldType.LinkMultiple;
            var xmlType = fieldPattern.FieldType.GetXmlType();
            Assert.True(fieldPattern.FieldType.IsLink);
            Assert.True(FieldType.IsXmlTypeLink(xmlType));

            var fieldType = FieldType.GetFromXmlType(xmlType);
            Assert.True(fieldType == fieldPattern.FieldType);

        }

    }
}
