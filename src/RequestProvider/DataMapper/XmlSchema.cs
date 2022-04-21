using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace MtdKey.Storage
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

        public long GetVersion()
        {
            var version = xmlDocument?.DocumentElement.Attributes["version"].Value;
            return long.Parse(version);
        }

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

        public List<BunchSchema> GetBunches()
        {
            var result = new List<BunchSchema>();
            XmlElement root = xmlDocument.DocumentElement;
            var bunches = root.GetElementsByTagName("bunch");
            foreach (XmlNode bunch in bunches)
            {
                result.Add(new()
                {
                    Name = bunch.Attributes["name"].Value
                });                
            }
            return result;
        }

        public List<FieldTag> GetFields()
        {
            var result = new List<FieldTag>();
            XmlElement root = xmlDocument.DocumentElement;
            var fields = root.GetElementsByTagName("field");
            foreach (XmlNode field in fields)
            {
                var bunchName = field.ParentNode.Attributes["name"].Value;
                var fieldType = field.Attributes["type"].Value;
                var bunchList = field.Attributes["list"]?.Value;

                var fieldSchema = new FieldSchema()
                {
                    FieldType = FieldType.GetByName(fieldType),
                    Name = field.Attributes["name"].Value,
                };

                result.Add(new()
                {
                    BunchName = bunchName,
                    BunchList = bunchList,
                    FieldSchema = fieldSchema
                });
            }

            return result;
        }

    }
}
