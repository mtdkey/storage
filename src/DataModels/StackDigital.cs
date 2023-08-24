using System.ComponentModel.DataAnnotations;

namespace MtdKey.Storage.DataModels
{
    internal class StackDigital
    {
        [Key]
        public long StackId { get; set; }
        public decimal Value { get; set; }

        public virtual Stack Stack { get; set; }
    }
}
