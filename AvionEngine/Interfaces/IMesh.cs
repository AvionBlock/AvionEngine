using AvionEngine.Enums;
using System;

namespace AvionEngine.Interfaces
{
    public interface IMesh : IRenderable, IDisposable
    {
        IRenderer Renderer { get; }

        bool IsDisposed { get; }

        void Update<TVertex>(TVertex[] vertices, uint[] indices, UsageMode? usageMode = null, DrawMode? drawMode = null) where TVertex : unmanaged;
    }
}