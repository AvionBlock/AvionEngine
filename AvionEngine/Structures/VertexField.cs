using System;

namespace AvionEngine.Structures
{
    [AttributeUsage(AttributeTargets.Field)]
    public class VertexField : Attribute
    {
        public FormatType FormatType;
        public InputType InputType;

        public VertexField(FormatType formatType, InputType inputType)
        {
            FormatType = formatType;
            InputType = inputType;
        }
    }
}
