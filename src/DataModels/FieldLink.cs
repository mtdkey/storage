using System.ComponentModel.DataAnnotations;


namespace MtdKey.Storage.DataModels
{
    internal class FieldLink
    {
        [Key]
        public long FieldId { get; set; }
        public long BunchId { get; set; }
        public int LinkType { get; set; }

        public virtual Field Field { get; set; }
        public virtual Bunch Bunch { get; set; }

    }
}
