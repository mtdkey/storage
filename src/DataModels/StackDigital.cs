using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
