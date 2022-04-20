using MtdKey.Storage.DataMapper;
using MtdKey.Storage.Tests.HelperFunctions;
using System;
using System.Linq;
using System.Threading.Tasks;
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
            Assert.True(bunches.Keys.Where(x => x.Equals("company")).Any());
            Assert.True(bunches.Keys.Where(x => x.Equals("issueSubject")).Any());
        }

        [Fact]
        public void C_GetFields()
        {
            var dataMapper = new XmlSchema<T050_XMLSchema>();
            dataMapper.LoadSchemaFromServer();
            var fields = dataMapper.GetFields();
            Assert.True(fields.Where(x => x.XmlBunchId.Equals("company")).Any());
            Assert.True(fields.Where(x => x.XmlBunchId.Equals("issueSubject")).Any());
            Assert.True(fields.Where(x => x.FieldSchema.Name.Equals("TIN")).Any());
            Assert.True(fields.Where(x => x.FieldSchema.Name.Equals("Assigned to")).Any());

            var queryA = fields.Where(x => x.FieldSchema.FieldType is null);
            Assert.False(queryA.Any(),queryA.FirstOrDefault()?.FieldSchema.Description);

            var queryB = fields.Where(x => x.FieldSchema.FieldType == FieldType.Link && x.XmlLinkId == null);
            Assert.False(queryB.Any(), queryB.FirstOrDefault()?.FieldSchema.Description);


        }

    }
}
