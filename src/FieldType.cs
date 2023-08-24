using System.Collections.Generic;

namespace MtdKey.Storage
{
    public readonly struct FieldType
    {
        private readonly int value;
        public FieldType(int fieldType)
        {
            value = fieldType;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj is FieldType fieldType)
            {
                return fieldType.value == value;
            }

            if (obj is int intValue)
            {
                return intValue == value;
            }
            return false;
        }

        public static bool operator ==(int left, FieldType right)
        {
            return left == right.value;
        }

        public static bool operator !=(int left, FieldType right)
        {
            return left != right.value;
        }

        public static bool operator ==(FieldType left, FieldType right)
        {
            return left.value == right.value;
        }

        public static bool operator !=(FieldType left, FieldType right)
        {
            return left.value != right.value;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static implicit operator int(FieldType fieldType)
        {
            return fieldType.value;
        }

        public static implicit operator FieldType(int fieldType)
        {
            return new FieldType(fieldType);
        }

        public readonly static FieldType Numeric = 1;
        public readonly static FieldType Text = 2;
        public readonly static FieldType DateTime = 3;
        public readonly static FieldType Boolean = 4;
        public readonly static FieldType LinkSingle = 5;
        public readonly static FieldType File = 6;
        public readonly static FieldType LinkMultiple = 7;

        public string GetXmlType()
        {
            switch (value)
            {
                case 1: { return XmlType.Numeric; }
                case 2: { return XmlType.Text; }
                case 3: { return XmlType.DateTime; }
                case 4: { return XmlType.Boolean; }
                case 5: { return XmlType.LinkSingle; }
                case 6: { return XmlType.File; }
                case 7: { return XmlType.LinkMultiple; }
                default: return null;
            }
        }

        public static FieldType GetFromXmlType(string xmlType)
        {
            var name = xmlType.ToLower();
            switch (name)
            {
                case XmlType.Numeric: { return Numeric; }
                case XmlType.DateTime: { return DateTime; }
                case XmlType.Boolean: { return Boolean; }
                case XmlType.LinkSingle: { return LinkSingle; }
                case XmlType.LinkMultiple: { return LinkMultiple; }
                case XmlType.File: { return File; }
                default: return Text;
            }
        }


        public static bool IsXmlTypeLink(string xmlType) => xmlType.Contains("link-");
        public bool IsLink => value == LinkSingle || value == LinkMultiple;


        public static List<string> XmlTypes => new()
        {
            XmlType.Numeric,
            XmlType.Text,
            XmlType.DateTime,
            XmlType.Boolean,
            XmlType.LinkSingle,
            XmlType.LinkMultiple,
            XmlType.File
        };

    }
}
