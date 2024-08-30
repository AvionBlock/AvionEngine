namespace AvionEngine.Structures
{
    public struct TextureInfo
    {
        public FormatType Format;
        public uint Width;
        public uint Height;
        public uint Depth;
        public uint MipLevels;
        public byte[]?[] Data;
    }
}
