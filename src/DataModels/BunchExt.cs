using System.ComponentModel.DataAnnotations;

namespace MtdKey.Storage.DataModels
{
    internal class BunchExt
    {
        [Key]
        public long BunchId { get; set; }
        public int Counter { get; set; }
        public virtual Bunch Bunch { get; set; }

    }
}
