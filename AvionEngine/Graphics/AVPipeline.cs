using System;
using System.Collections.Generic;

namespace AvionEngine.Graphics
{
    public abstract class AVPipeline : IDisposable
    {
        public bool IsDisposed { get; protected set; }
        public readonly List<uint> Shaders = new List<uint>();

        public abstract void AddShader(AVShader shader);

        public abstract void Dispose();
    }
}
