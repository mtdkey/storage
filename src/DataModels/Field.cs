using MtdKey.Storage.Context;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MtdKey.Storage.DataModels
{ 
    internal class Field 
    {
        public Field()
        {
            Stacks = new HashSet<Stack>();
        }

        [Key]
        public long Id { get; set; }
        public long BunchId { get; set; }
        public string Name { get; set; }
        public int FieldType { get; set; }        
        public byte DeletedFlag { get; set; }

        public virtual Bunch Bunch { get; set; }
        public virtual FieldLink FieldLink { get; set; }
        public virtual ICollection<Stack> Stacks { get; set; }
        
    }
}
