using Arch.Core;
using AvionEngine.Enums;
using AvionEngine.Rendering;

namespace AvionEngine.Interfaces
{
    public interface IEngine
    {
        IRenderer Renderer { get; }

        BaseShader CreateShader(string vertex, string fragment);

        BaseMesh<TVertex> CreateMesh<TVertex>(TVertex[] vertices, uint[] indices, DrawMode drawMode = DrawMode.Static) where TVertex : unmanaged;

        World World { get; }
    }
}
