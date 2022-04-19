using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public class DataMapper
    {
        public void ReadFile() {
            var assembly = Assembly.GetExecutingAssembly();           
            string resourceName = typeof(DataMapper).Namespace + $"./DataSchema/BunchSchema.xml";         
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            ReadFile(stream);
        }

        public string ReadFile(Stream stream) {
            using StreamReader reader = new(stream);
            string fileText = reader.ReadToEnd();
            return fileText;
        }
    }
}
