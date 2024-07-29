using System;

namespace AvionEngine.Interfaces
{
    public interface IMesh : IRenderable, IDisposable
    {
        bool IsDisposed { get; }

        void Set<T>(T[] vertices, uint[] indices) where T : unmanaged;
    }
}