using System;

namespace AvionEngine.Interfaces
{
    public interface IMesh<TVertex> : IRenderable, IDisposable where TVertex : unmanaged
    {
        bool IsDisposed { get; }

        void Set(TVertex[] vertices, uint[] indices, int offset = 0);
    }
}