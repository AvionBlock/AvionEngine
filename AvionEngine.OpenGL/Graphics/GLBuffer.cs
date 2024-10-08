﻿using AvionEngine.Graphics;
using Silk.NET.OpenGL;
using System;

namespace AvionEngine.OpenGL.Graphics
{
    public class GLBuffer : AVBuffer
    {
        public readonly GLRenderer Renderer;

        public readonly uint Buffer;

        public unsafe GLBuffer(GLRenderer renderer, BufferType bufferType, BufferUsageMode bufferUsageMode, uint sizeInBytes, void* data) : base(bufferType, bufferUsageMode)
        {
            Renderer = renderer;
            Buffer = renderer.glContext.GenBuffer();
            Bind();
            renderer.glContext.BufferData(GetBufferTargetARB(bufferType), (UIntPtr)sizeInBytes, data, GetBufferUsageARB(BufferUsageMode));

            //Unbind to be safe.
            Unbind();
        }

        public void Bind()
        {
            Renderer.glContext.BindBuffer(GetBufferTargetARB(BufferType), Buffer);
        }

        public void Unbind()
        {
            Renderer.glContext.BindBuffer(GetBufferTargetARB(BufferType), 0);
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
                Renderer.glContext.DeleteBuffer(Buffer);
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
