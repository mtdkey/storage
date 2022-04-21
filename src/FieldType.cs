using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public class FieldType
    {
        private readonly int value;
        public FieldType(int fieldType)
        {
            value = fieldType;
        }

        public static explicit operator int(FieldType fieldType)
        {
            return fieldType.value;
        }

        public static implicit operator FieldType(int fieldType)
        {
            return new FieldType(fieldType);
        }

        public readonly static FieldType Numeric = new(1);
        public readonly static FieldType Text = new(2);
        public readonly static FieldType DateTime = new(3);
        public readonly static FieldType Boolean = new(4);
        public readonly static FieldType Link = new(5);
        public readonly static FieldType File = new(6);

        public static string GetName(FieldType fieldType)
        {
            switch ((int)fieldType)
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
                default: return null;
            }
        }

    }
}
