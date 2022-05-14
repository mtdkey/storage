using System;
using System.Collections.Generic;


namespace MtdKey.Storage
{    
    public class NodePattern
    {
        public long NodeId { get; set; }
        public long BunchId { get; set; }
        public int Number { get; set; }
        public string CreatorInfo { get; set; }
        public DateTime DateCreated { get; set; }

        public List<NodePatternItem> Items { get; set; } = new List<NodePatternItem>();

    }
}
