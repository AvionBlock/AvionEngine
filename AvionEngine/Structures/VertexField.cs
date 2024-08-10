using AvionEngine.Enums;
using System;

namespace AvionEngine.Structures
{
    [AttributeUsage(AttributeTargets.Field)]
    public class VertexField : Attribute
    {
        public FieldType FieldType;

        public VertexField(FieldType fieldType)
        {
            FieldType = fieldType;
        }
    }
}
