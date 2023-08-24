namespace MtdKey.Storage
{
    public class FileData
    {
        public long StackId { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string Mime { get; set; }
        public byte[] ByteArray { get; set; }

    }
}
