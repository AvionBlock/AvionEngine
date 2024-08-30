﻿using AvionEngine.Rendering;
using Silk.NET.OpenGL;
using System;

namespace AvionEngine.OpenGL.Rendering
{
    public class GLBuffer : AVBuffer
    {
        public readonly Renderer renderer;

        public readonly uint Buffer;

        public GLBuffer(Renderer renderer, BufferType bufferType) : base(bufferType)
        {
            this.renderer = renderer;
            Buffer = renderer.glContext.GenBuffer();
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                renderer.glContext.DeleteBuffer(Buffer);
            }

            IsDisposed = true;
        }

        ~GLBuffer()
        {
            Dispose(false);
        }

        public static BufferTargetARB GetBufferTargetARB(BufferType bufferType)
        {
            return bufferType switch
            {
                BufferType.Vertex => BufferTargetARB.ArrayBuffer,
                BufferType.Index => BufferTargetARB.ElementArrayBuffer,
                BufferType.Constant => BufferTargetARB.UniformBuffer,
                _ => throw new ArgumentOutOfRangeException(nameof(bufferType))
            };
        }

        public static BufferUsageARB GetBufferUsageARB(BufferUsageMode bufferUsageMode)
        {
            return bufferUsageMode switch
            {
                BufferUsageMode.Static => BufferUsageARB.StaticDraw,
                BufferUsageMode.Dynamic => BufferUsageARB.DynamicDraw,
                BufferUsageMode.Stream => BufferUsageARB.StreamDraw,
                _ => throw new ArgumentOutOfRangeException(nameof(bufferUsageMode))
            };
        }
    }
}
