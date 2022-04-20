using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public class FieldSchema
    {
        public long FieldId { get; set; }
        public long BunchId { get; set; }
        /// <summary>
        /// BunchId who is a dictionary for this field
        /// </summary>
        public long LinkId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public FieldType FieldType { get; set; }
        public bool ArchiveFlag { get; set; }
    }
}
