using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace MtdKey.Storage.Tests.TestFiles
{
    public static class FileReader
    {
        public static async Task<byte[]> GetFileBytesAsync()
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = typeof(FileReader).Namespace + $".LongText.txt";
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            byte[] buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer);
            return buffer;
        }

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

        public static string GetOldSchema()
        {
            return GetFileText("Schemas.OldSchema.xml");
        }

        public static string GetNewSchema()
        {
            return GetFileText("Schemas.NewSchema.xml");
        }
    }
}
