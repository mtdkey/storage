﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public class FileData
    {
        public long StackId { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string Mime { get; set; }
        public byte[] ByteArray { get; set; }

    }
}
