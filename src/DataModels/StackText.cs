using System.ComponentModel.DataAnnotations;

namespace MtdKey.Storage.DataModels
{
    internal class StackText
    {
        [Key]
        public long Id { get; set; }
        public long StackId { get; set; }
        public string Value { get; set; }

        public virtual Stack Stack { get; set; }
    }
}
