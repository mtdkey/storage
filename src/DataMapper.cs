using MtdKey.Storage.DataModels;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

namespace MtdKey.Storage
{
    public class DataMapper
    {        
        public static string ReadDataFromFile()
        {            
            var assembly = Assembly.GetCallingAssembly();          
            string resourceName = assembly.GetName().Name + $".DataSchema.BunchesSchema.xml";         
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader reader = new(stream);
            string fileText = reader.ReadToEnd();
            return fileText;
        }  

        private RequestProvider requestProvider { get; set; }

        public DataMapper (RequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<bool> GetBunchesAsync()
        {
            var xmlDocument = new XmlDocument();
            var xmlData = ReadDataFromFile();
            xmlDocument.LoadXml(xmlData);

            XmlElement root = xmlDocument.DocumentElement;
            var bunches = root.GetElementsByTagName("bunch");
            foreach (XmlNode bunch in bunches)
            {                
                var requestResult = await requestProvider.BunchSaveAsync(schema => {
                    schema.SchemaId = bunch.Attributes["id"].Value;
                    schema.Name = bunch["name"].InnerText;
                    schema.Description = bunch["description"].InnerText;
                });

                var bunchId = requestResult.DataSet[0].BunchId;
                var fields = bunch.SelectNodes("/field");

                foreach(var field in fields)
                {
                    await requestProvider.FieldSaveAsync(schema => { 
                        schema.BunchId = bunchId;
                        schema.FieldType = FieldType.Text;   

                    });
                }
            }
            

            return true;

        }       

    }
}
