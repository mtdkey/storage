namespace MtdKey.Storage
{
    public class FieldPattern
    {
        public long FieldId { get; set; }
        public long BunchId { get; set; }
        /// <summary>
        /// BunchId who is a dictionary for this field
        /// </summary>
        public long LinkId { get; set; }
        public string Name { get; set; }
        public FieldType FieldType { get; set; } = FieldType.Text;
    }
}
