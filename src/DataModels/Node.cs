using MtdKey.Storage.Context;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MtdKey.Storage.DataModels
{
    internal class Node : IFilterBasic, IFilterChild
    {
        public Node()
        {
            Stacks = new HashSet<Stack>();
            StackLists = new HashSet<StackList>();
        }

        [Key]
        public long Id { get; set; }
        /// <summary>
        /// Bunch ID value
        /// </summary>
        public long BunchId { get; set; }        
        public byte ArchiveFlag { get; set; }
        public byte DeletedFlag { get; set; }

        public virtual Bunch Bunch {get;set;}
        public virtual NodeExt NodeExt { get; set; }
        public virtual NodeToken NodeToken { get; set; }
        public virtual ICollection<Stack> Stacks { get; set; }
        public virtual ICollection<StackList> StackLists { get; set; }

    }
}
