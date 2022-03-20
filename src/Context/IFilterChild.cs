using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage.Context
{
    internal interface IFilterChild
    {
        public long ParentId { get; set; }

    }
}
