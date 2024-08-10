using AvionEngine.Enums;
using System;

namespace AvionEngine.Interfaces
{
    public interface IMesh : IRenderable, IDisposable
    {
        bool IsDisposed { get; }

        DrawMode DrawMode { get; set; }

        void Update<TVertex>(TVertex[] vertices, uint[] indices) where TVertex : unmanaged;
    }
}