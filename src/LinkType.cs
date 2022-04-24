using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    
    public struct LinkType
    {
        private readonly int value;
        public LinkType(int linkType)
        {
            value = linkType;
        }

        public static explicit operator int(LinkType linkType)
        {
            return linkType.value;
        }

        public static implicit operator LinkType(int linkType)
        {
            return new LinkType(linkType);
        }

        public readonly static LinkType Single = 1;
        public readonly static LinkType Multiple = 2;        

    }
}
