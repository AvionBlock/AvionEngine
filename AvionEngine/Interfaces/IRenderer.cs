using AvionEngine.Enums;
using AvionEngine.Structures;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using System.Drawing;

namespace AvionEngine.Interfaces
{
    public interface IRenderer
    {
        IWindow Window { get; }

        IShader CreateShader(string vertex, string fragment);

        IMesh CreateMesh<TVertex>(TVertex[] vertices, uint[] indices, UsageMode usageMode = UsageMode.Static, DrawMode drawMode = DrawMode.Triangles) where TVertex : unmanaged;

        ITexture CreateTexture(
            TextureInfo textureData,
            TextureTargetMode targetMode = TextureTargetMode.Texture2D,
            TextureFormatMode formatMode = TextureFormatMode.RGB,
            WrapMode wrapModeS = WrapMode.Repeat,
            WrapMode wrapModeT = WrapMode.Repeat,
            WrapMode wrapModeR = WrapMode.Repeat,
            MinFilterMode minFilterMode = MinFilterMode.Linear,
            MagFilterMode magFilterMode = MagFilterMode.Linear);

        ITexture CreateTexture(
            TextureInfo[] textureData,
            TextureTargetMode targetMode = TextureTargetMode.Texture2D,
            TextureFormatMode formatMode = TextureFormatMode.RGB,
            WrapMode wrapModeS = WrapMode.Repeat,
            WrapMode wrapModeT = WrapMode.Repeat,
            WrapMode wrapModeR = WrapMode.Repeat,
            MinFilterMode minFilterMode = MinFilterMode.Linear,
            MagFilterMode magFilterMode = MagFilterMode.Linear);

        void Resize(Vector2D<int> newSize);
        
        void SetClearColor(Color color);

        void Clear();
    }
}
