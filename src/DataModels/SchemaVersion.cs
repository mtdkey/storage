using System.ComponentModel.DataAnnotations;

namespace MtdKey.Storage.DataModels
{
    internal class SchemaVersion
    {
        [Key]
        public long Id { get; set; }
        public long SchemaNameId { get; set; }
        public long Version { get; set; }
        public string XmlSchema { get; set; }

        public virtual SchemaName SchemaName { get; set; }

    }
}
