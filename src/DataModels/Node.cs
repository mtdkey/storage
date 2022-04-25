using MtdKey.Storage.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MtdKey.Storage.DataModels
{
    internal class Node : IFilterBasic, IFilterChild
    {
        public Node()
        {
            Stacks = new HashSet<Stack>();
            StackLists = new HashSet<StackLink>();
        }

        [Key]
        public long Id { get; set; }
        public long BunchId { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatorInfo { get; set; }
        public byte DeletedFlag { get; set; }

        public virtual Bunch Bunch {get;set;}
        public virtual NodeExt NodeExt { get; set; }
        public virtual NodeToken NodeToken { get; set; }
        public virtual ICollection<Stack> Stacks { get; set; }
        public virtual ICollection<StackLink> StackLists { get; set; }

    }
}
