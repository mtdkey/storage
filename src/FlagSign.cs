using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage
{        
    internal static class FlagSign
    {
        public readonly static byte True = 1;
        public readonly static byte False = 0;

        public static byte AsFlagSign(this bool flag)
        {
            return flag is true ? True : False;
        }

        public static bool AsBoolean(this byte flag)
        {
            return flag.Equals(True);
        }

    }
}
