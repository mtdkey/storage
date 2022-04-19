using MtdKey.Storage.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MtdKey.Storage.DataModels
{
    internal class Bunch : IFilterBasic
    {
        public Bunch()
        {
            Fields = new HashSet<Field>();
            Nodes = new HashSet<Node>();
        }

        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SchemaId { get; set; } = Guid.NewGuid().ToString();
        public byte ArchiveFlag { get; set; }
        public byte DeletedFlag { get; set; }

        public virtual BunchExt BunchExt { get; set; }
        public virtual BunchToken BunchToken { get; set; }
        public virtual ICollection<Field> Fields { get; set; }
        public virtual ICollection<Node> Nodes { get; set; }
    }
}
