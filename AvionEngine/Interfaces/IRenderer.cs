using AvionEngine.Enums;
using AvionEngine.Rendering;
using AvionEngine.Structures;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using System;
using System.Drawing;

namespace AvionEngine.Interfaces
{
    public interface IRenderer
    {
        IWindow Window { get; }

        BaseShader CreateShader(string vertex, string fragment);

        BaseMesh CreateMesh<TVertex>(TVertex[] vertices, uint[] indices, UsageMode usageMode = UsageMode.Static, DrawMode drawMode = DrawMode.Triangles) where TVertex : unmanaged;

        BaseTexture CreateTexture(
            TextureInfo textureData,
            TextureTargetMode targetMode = TextureTargetMode.Texture2D,
            FormatType formatMode = FormatType.RGB,
            WrapMode wrapModeS = WrapMode.Repeat,
            WrapMode wrapModeT = WrapMode.Repeat,
            WrapMode wrapModeR = WrapMode.Repeat,
            MinFilterMode minFilterMode = MinFilterMode.Linear,
            MagFilterMode magFilterMode = MagFilterMode.Linear);

        BaseTexture CreateTexture(
            TextureInfo[] textureData,
            TextureTargetMode targetMode = TextureTargetMode.Texture2D,
            FormatType formatMode = FormatType.RGB,
            WrapMode wrapModeS = WrapMode.Repeat,
            WrapMode wrapModeT = WrapMode.Repeat,
            WrapMode wrapModeR = WrapMode.Repeat,
            MinFilterMode minFilterMode = MinFilterMode.Linear,
            MagFilterMode magFilterMode = MagFilterMode.Linear);

        void Execute(Action action);

        void ExecuteQueue();

        void Resize(Vector2D<int> newSize);
        
        void SetClearColor(Color color);

        void SetDepthTest(bool enabled);

        void SetDepthMask(bool enabled);

        void SetWireframe(bool enabled);

        void Clear();
    }
}
