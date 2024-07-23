using AvionEngine.Structures;

namespace AvionEngine.Interfaces
{
    public interface IMesh : IRenderable
    {
        uint[] Indices { get; }

        void Set(Vertex[] vertices, uint[] indices);
    }
}