using System;

namespace AvionEngine.Graphics
{
    public abstract class AVBuffer : IDisposable
    {
        public readonly BufferType BufferType;
        public readonly BufferUsageMode BufferUsageMode;

        public bool IsDisposed { get; protected set; }

        public AVBuffer(BufferType bufferType, BufferUsageMode bufferUsageMode)
        {
            BufferType = bufferType;
            BufferUsageMode = bufferUsageMode;
        }

        public abstract void Dispose();
    }
}
