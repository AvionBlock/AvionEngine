using Arch.Core;
using AvionEngine.Enums;
using AvionEngine.Rendering;

namespace AvionEngine.Interfaces
{
    public interface IEngine
    {
        IRenderer Renderer { get; }

        BaseShader CreateShader(string vertex, string fragment);

        BaseMesh CreateMesh<TVertex>(TVertex[] vertices, uint[] indices, UsageMode drawMode = UsageMode.Static) where TVertex : unmanaged;

        World World { get; }
    }
}
