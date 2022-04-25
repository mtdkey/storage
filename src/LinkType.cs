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

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj is LinkType linkType)
            {
                return linkType.value == value;
            }

            if (obj is int intValue)
            {
                return intValue == value;
            }
            return false;
        }


        public static bool operator ==(int left, LinkType right)
        {
            return left == right.value;
        }

        public static bool operator !=(int left, LinkType right)
        {
            return left != right.value;
        }

        public static bool operator ==(LinkType left, LinkType right)
        {
            return left.value == right.value;
        }

        public static bool operator !=(LinkType left, LinkType right)
        {
            return left.value != right.value;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
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
