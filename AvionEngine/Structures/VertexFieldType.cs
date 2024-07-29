using System;

namespace AvionEngine.Structures
{
    [AttributeUsage(AttributeTargets.Field)]
    public class VertexFieldType : Attribute
    {
        public Type FieldType;

        public VertexFieldType(Type type)
        {
            if (!type.IsPrimitive)
                throw new ArgumentException($"Parameter {nameof(type)} must be a primitive!", nameof(type));
            FieldType = type;
        }
    }
}
