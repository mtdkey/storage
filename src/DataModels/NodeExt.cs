using System.ComponentModel.DataAnnotations;

namespace MtdKey.Storage.DataModels
{
    internal class NodeExt
    {
        [Key]
        public long NodeId { get; set; }
        public int Number { get; set; }

        public virtual Node Node { get; set; }
    }
}
