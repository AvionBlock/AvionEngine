using AvionEngine.Structures;
using System;

namespace AvionEngine.Interfaces
{
    public interface IMesh : IRenderable, IDisposable
    {
        bool IsDisposed { get; }

        void Set(Vertex[] vertices, uint[] indices);
    }
}