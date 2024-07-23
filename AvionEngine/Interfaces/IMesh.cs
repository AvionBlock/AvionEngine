using AvionEngine.Structures;

namespace AvionEngine.Interfaces
{
    public interface IMesh : IRenderable
    {
        void Set(Vertex[] vertices, uint[] indices);
    }
}