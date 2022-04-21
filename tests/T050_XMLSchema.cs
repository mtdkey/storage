using System.Linq;
using Xunit;

namespace MtdKey.Storage.Tests
{
    [Collection("Sequential")]
    public class T050_XMLSchema
    {
        [Fact]
        public void A_ReadDataFromFile()
        {                       
            var dataMapper = new XmlSchema<T050_XMLSchema>();
            var data = dataMapper.ReadDataFromFile();
            Assert.NotNull(data);
        }

        [Fact]
        public void B_LoadSchemaFromServer()
        {            
            var dataMapper = new XmlSchema<T050_XMLSchema>();
            dataMapper.LoadSchemaFromServer();
            var xmlDoc = dataMapper.GetXmlDocument();
            Assert.NotNull(xmlDoc);
        }

        [Fact]
        public void C_GetBunches()
        {
            var dataMapper = new XmlSchema<T050_XMLSchema>();
            dataMapper.LoadSchemaFromServer();
            var bunches = dataMapper.GetBunches();                    
            Assert.True(bunches.Where(x => x.Name.Equals("Company")).Any());
            Assert.True(bunches.Where(x => x.Name.Equals("IssueSubject")).Any());
        }

        [Fact]
        public void C_GetFields()
        {
            var dataMapper = new XmlSchema<T050_XMLSchema>();
            dataMapper.LoadSchemaFromServer();
            var fields = dataMapper.GetFields();
            Assert.True(fields.Where(x => x.BunchName.Equals("Company")).Any());
            Assert.True(fields.Where(x => x.BunchName.Equals("IssueSubject")).Any());
            Assert.True(fields.Where(x => x.FieldSchema.Name.Equals("TIN")).Any());
            Assert.True(fields.Where(x => x.FieldSchema.Name.Equals("Assigned to")).Any());

            var queryA = fields.Where(x => x.FieldSchema.FieldType is null);
            Assert.False(queryA.Any(),queryA.FirstOrDefault()?.FieldSchema.Name);

            var queryB = fields.Where(x => x.FieldSchema.FieldType == FieldType.Link && x.ListBunch == null);
            Assert.False(queryB.Any(), queryB.FirstOrDefault()?.FieldSchema.Name);


        }

    }
}
