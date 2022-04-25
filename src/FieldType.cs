using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public struct FieldType
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
        public readonly static FieldType Link = 5;
        public readonly static FieldType File = 6;

        public static string GetName(FieldType fieldType)
        {
            switch (fieldType)
            {
                case 1: { return nameof(Numeric); }
                case 2: { return nameof(Text); }
                case 3: { return nameof(DateTime); }
                case 4: { return nameof(Boolean); }
                case 5: { return nameof(Link); }
                case 6: { return nameof(File); }
                default: return null;
            }
        }

        public static FieldType GetByName(string typeName)
        {
            var name = typeName.ToLower();
            switch (name)
            {
                case "numeric": { return Numeric; }
                case "text": { return Text; }
                case "datetime": { return DateTime; }
                case "boolean": { return Boolean; }
                case "link": { return Link; }
                case "file": { return File; }
                default: return Text;
            }
        }

        public static List<FieldType> AllTypes => new()
        {
            Numeric,
            Text,
            DateTime,
            Boolean,
            Link,
            File
        };

    }
}
