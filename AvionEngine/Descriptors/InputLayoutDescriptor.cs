namespace AvionEngine.Descriptors
{
    public struct InputLayoutDescriptor
    {
        public readonly FormatType FormatType;
        public readonly uint Offset;
        public readonly uint Slot;
        public readonly InputType InputType;

        public InputLayoutDescriptor(FormatType formatType, uint offset, uint slot, InputType inputType)
        {
            FormatType = formatType;
            Offset = offset;
            Slot = slot;
            InputType = inputType;
        }
    }
}
