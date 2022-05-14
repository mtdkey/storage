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
    public class XmlSchema<T>: IXmlSchema where T : class
    {
        public string ReadDataFromFile()
        {
            var assembly = Assembly.GetAssembly(typeof(T));
            string resourceName = assembly.GetName().Name + $".Schemas.{nameSchema}._Schema.xml";
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader reader = new(stream);
            string fileText = reader.ReadToEnd();
            return fileText;
        }

        private XmlDocument xmlDocument;
        private readonly string nameSchema;

        public XmlSchema(string nameSchema) {
            this.nameSchema = nameSchema;
        }

        public string GetName()
        {
            var name = xmlDocument?.DocumentElement.Attributes["name"].Value;
            return name;
        }

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

        public List<BunchPattern> GetBunches()
        {
            var result = new List<BunchPattern>();
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

                var FieldPattern = new FieldPattern()
                {
                    FieldType = FieldType.GetFromXmlType(fieldType),
                    Name = field.Attributes["name"].Value,
                };

                result.Add(new()
                {
                    BunchName = bunchName,
                    BunchList = bunchList,
                    FieldPattern = FieldPattern
                });
            }

            return result;
        }

    }
}
