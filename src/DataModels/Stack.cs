﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MtdKey.Storage.DataModels
{
    internal class Stack
    {
        public Stack()
        {
            StackTexts = new HashSet<StackText>();
            StackLists = new HashSet<StackLink>();
            StackFiles = new HashSet<StackFile>();
        }

        [Key]
        public long Id { get; set; }
        public long NodeId { get; set; }
        public long FieldId { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatorInfo { get; set; }

        public virtual StackDigital StackDigital { get; set; }
        public virtual Node Node { get; set; }
        public virtual Field Field { get; set; }
        public virtual ICollection<StackText> StackTexts { get; set; }
        public virtual ICollection<StackLink> StackLists { get; set; }
        public virtual ICollection<StackFile> StackFiles { get; set; }
    }
}
