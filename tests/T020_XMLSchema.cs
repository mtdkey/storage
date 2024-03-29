﻿using System.Linq;
using Xunit;

namespace MtdKey.Storage.Tests
{

    public class T020_XMLSchema
    {
        [Fact]
        public void A_ReadDataFromFile()
        {
            var dataMapper = new XmlSchema<T020_XMLSchema>();
            var data = dataMapper.ReadDataFromFile("Issue");
            Assert.NotNull(data);
        }

        [Fact]
        public void B_LoadSchemaFromServer()
        {
            var dataMapper = new XmlSchema<T020_XMLSchema>();
            dataMapper.LoadSchemaFromServer("Issue");
            var xmlDoc = dataMapper.GetXmlDocument();
            Assert.NotNull(xmlDoc);
            var name = dataMapper.GetName();
            Assert.Equal("Issue", name);
        }

        [Fact]
        public void C_GetBunches()
        {
            var dataMapper = new XmlSchema<T020_XMLSchema>();
            dataMapper.LoadSchemaFromServer("Issue");
            var bunches = dataMapper.GetBunches();
            Assert.True(bunches.Where(x => x.Name.Equals("Issue")).Any());
            Assert.True(bunches.Where(x => x.Name.Equals("IssueCategory")).Any());
        }

        [Fact]
        public void C_GetFields()
        {
            var dataMapper = new XmlSchema<T020_XMLSchema>();
            dataMapper.LoadSchemaFromServer("Issue");
            var fields = dataMapper.GetFields();
            Assert.True(fields.Where(x => x.BunchName.Equals("IssueReport")).Any());
            Assert.True(fields.Where(x => x.FieldPattern.Name.Equals("AssignedTo")).Any());

            var queryB = fields.Where(x => x.FieldPattern.FieldType.Equals(FieldType.LinkSingle) && x.BunchList == null);
            Assert.False(queryB.Any(), queryB.FirstOrDefault()?.FieldPattern.Name);


        }

    }
}
