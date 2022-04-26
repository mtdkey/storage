using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public class NodePatternItem
    {
        public long NodeId { get; set; }
        public long FieldId { get; private set; }
        public FieldType FieldType { get; private set; }
        public object Data { get; private set; }
        public string CreatorInfo { get; private set; }
        public DateTime DateCreated { get; private set; }

        public NodePatternItem(object value, long fieldId, FieldType fieldType, string creatorInfo, DateTime dateTime)
        {
            Data = value;            
            FieldId = fieldId;
            FieldType = fieldType;
            CreatorInfo = creatorInfo;
            DateCreated = dateTime;
        }

        public NodePatternItem(string value, long fieldId, string creatorInfo, DateTime dateTime)
        {
            Data = value;
            FieldType = FieldType.Text;
            FieldId = fieldId;
            CreatorInfo = creatorInfo;
            DateCreated = dateTime;
        }

        public NodePatternItem(decimal value, long fieldId, string creatorInfo, DateTime dateTime)
        {
            FieldId = fieldId;
            Data = value;
            FieldType = FieldType.Numeric;
            CreatorInfo = creatorInfo;
            DateCreated = dateTime;
        }

        public NodePatternItem(DateTime value, long fieldId, string creatorInfo, DateTime dateTime)
        {
            FieldId = fieldId;
            Data = value;
            FieldType = FieldType.DateTime;
            CreatorInfo = creatorInfo;
            DateCreated = dateTime;
        }

        public NodePatternItem(bool value, long fieldId, string creatorInfo, DateTime dateTime)
        {
            FieldId = fieldId;
            Data = value;
            FieldType = FieldType.Boolean;
            CreatorInfo = creatorInfo;
            DateCreated = dateTime;
        }

        public NodePatternItem(List<NodePattern> value, long fieldId, string creatorInfo, DateTime dateTime)
        {
            FieldId = fieldId;
            Data = value;            
            FieldType = FieldType.LinkSingle;
            CreatorInfo = creatorInfo;
            DateCreated = dateTime;
        }

        public NodePatternItem(List<FileData> value, long fieldId, string creatorInfo, DateTime dateTime)
        {
            FieldId = fieldId;
            Data = value;
            FieldType = FieldType.File;
            CreatorInfo = creatorInfo;
            DateCreated = dateTime;
        }

    }
}
