using System.Collections.Generic;

namespace MtdKey.Storage
{
    public class BunchPattern
    {
        public long BunchId { get; set; }
        public string Name { get; set; }
        public List<FieldPattern> FieldPatterns { get; set; }
    }
}
