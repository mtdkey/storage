using System.ComponentModel.DataAnnotations;


namespace MtdKey.Storage.DataModels
{
    internal class StackFile
    {
        [Key]
        public long StackId { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }
        public byte[] Data { get; set; }

        public virtual Stack Stack { get; set; }
    }
}
