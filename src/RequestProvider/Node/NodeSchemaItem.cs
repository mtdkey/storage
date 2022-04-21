using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public class NodeSchemaItem
    {
        public long NodeId { get; set; }
        public long FieldId { get; private set; }
        public Type SystemType { get; private set; }
        public FieldType FieldType { get; private set; }
        public object Data { get; private set; }
        public string CreatorInfo { get; private set; }
        public DateTime DateCreated { get; private set; }

        public NodeSchemaItem(string value, long fieldId, string creatorInfo, DateTime dateTime)
        {
            Data = value;
            SystemType = typeof(string);
            FieldType = FieldType.Text;
            FieldId = fieldId;
            CreatorInfo = creatorInfo;
            DateCreated = dateTime;
        }

        public NodeSchemaItem(decimal value, long fieldId, string creatorInfo, DateTime dateTime)
        {
            FieldId = fieldId;
            Data = value;
            SystemType = typeof(decimal);
            FieldType = FieldType.Numeric;
            CreatorInfo = creatorInfo;
            DateCreated = dateTime;
        }

        public NodeSchemaItem(DateTime value, long fieldId, string creatorInfo, DateTime dateTime)
        {
            FieldId = fieldId;
            Data = value;
            SystemType = typeof(DateTime);
            FieldType = FieldType.DateTime;
            CreatorInfo = creatorInfo;
            DateCreated = dateTime;
        }

        public NodeSchemaItem(bool value, long fieldId, string creatorInfo, DateTime dateTime)
        {
            FieldId = fieldId;
            Data = value;
            SystemType = typeof(double);
            FieldType = FieldType.Boolean;
            CreatorInfo = creatorInfo;
            DateCreated = dateTime;
        }

        public NodeSchemaItem(NodeSchema value, long fieldId, string creatorInfo, DateTime dateTime)
        {
            FieldId = fieldId;
            Data = value;
            SystemType = typeof(NodeSchema);
            FieldType = FieldType.Link;
            CreatorInfo = creatorInfo;
            DateCreated = dateTime;
        }

        public NodeSchemaItem(byte[] value, long fieldId, string creatorInfo, DateTime dateTime)
        {
            FieldId = fieldId;
            Data = value;
            SystemType = typeof(NodeSchema);
            FieldType = FieldType.File;
            CreatorInfo = creatorInfo;
            DateCreated = dateTime;
        }

    }
}
