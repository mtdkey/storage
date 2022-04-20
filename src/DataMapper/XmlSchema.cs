using MtdKey.Storage.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

namespace MtdKey.Storage.DataMapper
{
    /// <summary>
    ///  Data mapper for xml schema 
    /// </summary>
    /// <typeparam name="T">The class is the parent in which the DataMapper is run</typeparam>
    public class XmlSchema<T> where T : class
    {
        public string ReadDataFromFile()
        {
            var assembly = Assembly.GetAssembly(typeof(T));
            string resourceName = assembly.GetName().Name + $".DataSchema.BunchesSchema.xml";
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader reader = new(stream);
            string fileText = reader.ReadToEnd();
            return fileText;
        }

        private XmlDocument xmlDocument;

        public XmlSchema() { }

        /// <summary>
        /// Read the schema from DataSchema folder form file BunchesSchema.xml on the solution near MtdKey.Storage.dll
        /// The BunchesSchema.xml file must be an embedded resource
        /// </summary>
        public void LoadSchemaFromServer()
        {
            xmlDocument = new XmlDocument();
            var xmlData = ReadDataFromFile();
            xmlDocument.LoadXml(xmlData);
        }

        /// <summary>
        /// Get the XML document with the data schema. 
        /// </summary>
        public XmlDocument GetXmlDocument()
        {
            return xmlDocument;
        }

        public Dictionary<string, BunchSchema> GetBunches()
        {
            var result = new Dictionary<string, BunchSchema>();

            XmlElement root = xmlDocument.DocumentElement;
            var bunches = root.GetElementsByTagName("bunch");
            foreach (XmlNode bunch in bunches)
            {
                var id = bunch.Attributes["id"].Value;
                var bunchSchema = new BunchSchema()
                {
                    Name = bunch["name"].InnerText,
                    Description = bunch["description"].InnerText
                };

                result.Add(id, bunchSchema);
            }
            return result;
        }

        public List<FieldXmlTag> GetFields()
        {
            var result = new List<FieldXmlTag>();

            XmlElement root = xmlDocument.DocumentElement;
            var fields = root.GetElementsByTagName("field");
            foreach (XmlNode field in fields)
            {
                var bunchId = field.ParentNode.Attributes["id"].Value;
                var fieldType = field.Attributes["type"].Value;
                var xmlLinkId = field.Attributes["list"]?.Value;

                var fieldSchema = new FieldSchema()
                {
                    FieldType = FieldType.GetByName(fieldType),
                    Name = field["name"].InnerText,
                    Description = field["description"].InnerText
                };

                result.Add(new FieldXmlTag
                {
                    XmlBunchId = bunchId,
                    XmlLinkId = xmlLinkId,
                    FieldSchema = fieldSchema
                });
            }

            return result;
        }

    }
}
