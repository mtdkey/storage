using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage.DataModels
{
    internal class StackLink
    {
        [Key]
        public long Id { get; set; }
        public long StackId { get; set; }
        public long NodeId { get; set; }

        public virtual Stack Stack { get; set; }
        public virtual Node Node { get; set; }
    }
}
