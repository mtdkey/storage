using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
    }
}
