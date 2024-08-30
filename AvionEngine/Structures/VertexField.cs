using System;

namespace AvionEngine.Structures
{
    [AttributeUsage(AttributeTargets.Field)]
    public class VertexField : Attribute
    {
        public FormatType FieldType;

        public VertexField(FormatType fieldType)
        {
            FieldType = fieldType;
        }
    }
}
