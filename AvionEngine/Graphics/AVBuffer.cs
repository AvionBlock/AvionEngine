using System;

namespace AvionEngine.Graphics
{
    public abstract class AVBuffer : IDisposable
    {
        public readonly BufferType BufferType;

        public bool IsDisposed { get; protected set; }

        public AVBuffer(BufferType bufferType)
        {
            BufferType = bufferType;
        }

        public abstract void Dispose();
    }
}
