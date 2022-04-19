using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public class BunchSchema
    {
        public long BunchId {get;set;}
        public string Name { get; set; }
        public string SchemaId { get; set; }
        public string Description { get; set; }
        public bool ArchiveFlag { get; set; }

    }
}
