using System;

namespace AvionEngine.Interfaces
{
    public interface IMesh : IRenderable, IDisposable
    {
        bool IsDisposed { get; }

        void Set<TVert>(TVert[] vertices, uint[] indices, int offset = 0) where TVert : unmanaged;

        Type GetVertexType();
    }
}