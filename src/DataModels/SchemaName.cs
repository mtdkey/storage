using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage.DataModels
{
    internal class SchemaName
    {
        public SchemaName()
        {
            SchemaVersions = new HashSet<SchemaVersion>();
        }

        [Key]
        public long Id { get; set; }
        public string UniqueName { get; set; }

        public virtual ICollection<SchemaVersion> SchemaVersions { get; set; }
    }
}
