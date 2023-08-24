using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MtdKey.Storage.DataModels
{
    internal class Bunch
    {
        public Bunch()
        {
            Fields = new HashSet<Field>();
            FieldLinks = new HashSet<FieldLink>();
            Nodes = new HashSet<Node>();
        }

        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public byte DeletedFlag { get; set; }

        public virtual BunchExt BunchExt { get; set; }
        public virtual BunchToken BunchToken { get; set; }
        public virtual ICollection<Field> Fields { get; set; }
        public virtual ICollection<Node> Nodes { get; set; }
        public virtual ICollection<FieldLink> FieldLinks { get; set; }
    }
}
