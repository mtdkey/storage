using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage.Tests.TestFiles
{
    public static class FileReader
    {
        private static string GetFileText(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = typeof(FileReader).Namespace + $".{fileName}";
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader reader = new(stream);
            string fileText = reader.ReadToEnd();
            return fileText;
        }

        public static string GetShortText()
        {
            return GetFileText("ShortText.txt");
        }
        public static string GetLongText()
        {
            return GetFileText("LongText.txt");
        }
    }
}
