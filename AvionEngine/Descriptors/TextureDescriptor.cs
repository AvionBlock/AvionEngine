namespace AvionEngine.Descriptors
{
    public struct TextureDescriptor
    {
        public readonly FormatType FormatType;
        public readonly uint Width;
        public readonly uint Height;
        public readonly uint Depth;
        public readonly uint MipLevels;
        public readonly byte[]?[] Data;

        public TextureDescriptor(FormatType formatType, uint width, uint height, uint depth, uint mipLevels, byte[]?[] data)
        {
            FormatType = formatType;
            Width = width;
            Height = height;
            Depth = depth;
            MipLevels = mipLevels;
            Data = data;
        }
    }
}
