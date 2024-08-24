using AvionEngine.Enums;
using AvionEngine.Interfaces;
using System;

namespace AvionEngine.Rendering
{
    public abstract class BaseMesh : IRenderable, IDisposable
    {
        public abstract IRenderer Renderer { get; }

        public bool IsDisposed { get; protected set; }

        public abstract void Update<TVertex>(TVertex[] vertices, uint[] indices, UsageMode? usageMode = null, DrawMode? drawMode = null) where TVertex : unmanaged;

        public abstract void UpdateVertexType<TVertex>() where TVertex : unmanaged;

        public abstract void Render(double delta);

        public abstract void Dispose();
    }
}
