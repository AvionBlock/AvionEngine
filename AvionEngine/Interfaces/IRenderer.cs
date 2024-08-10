using AvionEngine.Enums;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using System.Drawing;

namespace AvionEngine.Interfaces
{
    public interface IRenderer
    {
        IWindow Window { get; }

        Color ClearColor { get; set; }

        IShader CreateShader(string vertex, string fragment);

        IMesh CreateMesh<TVertex>(TVertex[] vertices, uint[] indices, UsageMode drawMode = UsageMode.Static) where TVertex : unmanaged;

        ITexture CreateTexture2D(uint width, uint height, byte[] data, TextureFormat format = TextureFormat.RGB);

        void Resize(Vector2D<int> newSize);

        void Clear();
    }
}
