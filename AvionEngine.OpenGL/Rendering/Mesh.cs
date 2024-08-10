﻿using AvionEngine.Interfaces;
using Silk.NET.OpenGL;
using System.Runtime.InteropServices;
using System;
using System.Reflection;
using AvionEngine.Structures;
using AvionEngine.Enums;
using System.Linq;

namespace AvionEngine.OpenGL.Rendering
{
    public class Mesh : IMesh
    {
        private GL glInstance;
        private uint VBO;
        private uint VAO;
        private uint EBO;
        private uint indicesLength;
        private bool disposed;
        private DrawMode drawMode;
        private UsageMode usageMode;
        private Type? vertexType;

        public bool IsDisposed { get => disposed; }

        public unsafe Mesh(GL glInstance, UsageMode usageMode = UsageMode.Static, DrawMode drawMode = DrawMode.Triangles)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(Mesh));

            this.glInstance = glInstance;
            this.usageMode = usageMode;
            this.drawMode = drawMode;

            //Create Buffers and Arrays
            VAO = glInstance.GenVertexArray();
            VBO = glInstance.GenBuffer();
            EBO = glInstance.GenBuffer();
        }

        public unsafe void Update<TVertex>(TVertex[] vertices, uint[] indices, UsageMode? usageMode = null, DrawMode? drawMode = null) where TVertex : unmanaged
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(Mesh));
            this.usageMode = usageMode ?? this.usageMode;
            this.drawMode = drawMode ?? this.drawMode;

            glInstance.BindVertexArray(VAO);

            //Load data
            glInstance.BindBuffer(BufferTargetARB.ArrayBuffer, VBO);
            fixed (void* verticesPtr = vertices)
                glInstance.BufferData(BufferTargetARB.ArrayBuffer, (UIntPtr)(vertices.Length * sizeof(TVertex)), verticesPtr, GetBufferUsageARB(this.usageMode));

            glInstance.BindBuffer(BufferTargetARB.ElementArrayBuffer, EBO);
            fixed (uint* indicesPtr = indices)
                glInstance.BufferData(BufferTargetARB.ElementArrayBuffer, (UIntPtr)(indices.Length * sizeof(uint)), indicesPtr, GetBufferUsageARB(this.usageMode));

            if(!typeof(TVertex).Equals(vertexType))
                UpdateVertexType<TVertex>();

            glInstance.BindVertexArray(0);
            glInstance.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
            glInstance.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);
            indicesLength = (uint)indices.Length;
        }

        public unsafe void UpdateVertexType<TVertex>() where TVertex : unmanaged
        {
            var verticeFields = typeof(TVertex).GetFields().Where(x => Attribute.IsDefined(x, typeof(VertexField))).ToArray();

            for (uint i = 0; i < verticeFields.Length; i++)
            {
                var fieldSize = verticeFields[i].FieldType.GetFields().Length;
                if (fieldSize <= 0)
                    fieldSize = 1;

                glInstance.EnableVertexAttribArray(i);
                glInstance.VertexAttribPointer(
                    i,
                    fieldSize,
                    GetVertexAttribPointerType(verticeFields[i].GetCustomAttribute<VertexField>().FieldType),
                    false,
                    (uint)sizeof(TVertex),
                    (void*)Marshal.OffsetOf<TVertex>(verticeFields[i].Name));
            }

            vertexType = typeof(TVertex);
        }

        public unsafe void Render(double delta)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(Mesh));

            Console.WriteLine(usageMode);
            glInstance.BindVertexArray(VAO);
            glInstance.DrawElements(GetPrimitiveType(drawMode), indicesLength, DrawElementsType.UnsignedInt, (void*)0);
            glInstance.BindVertexArray(0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                glInstance.DeleteVertexArray(VAO);
                glInstance.DeleteBuffer(VBO);
                glInstance.DeleteBuffer(EBO);
            }

            disposed = true;
        }

        ~Mesh()
        {
            Dispose(false);
        }

        private static VertexAttribPointerType GetVertexAttribPointerType(FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.SByte:
                    return VertexAttribPointerType.Byte;
                case FieldType.Byte:
                    return VertexAttribPointerType.UnsignedByte;
                case FieldType.Int16:
                    return VertexAttribPointerType.Short;
                case FieldType.UInt16:
                    return VertexAttribPointerType.UnsignedShort;
                case FieldType.Int32:
                    return VertexAttribPointerType.Int;
                case FieldType.UInt32:
                    return VertexAttribPointerType.UnsignedInt;
                case FieldType.Int64:
                    return VertexAttribPointerType.Int64Arb;
                case FieldType.UInt64:
                    return VertexAttribPointerType.UnsignedInt64Arb;
                case FieldType.Single:
                    return VertexAttribPointerType.Float;
                case FieldType.Double:
                    return VertexAttribPointerType.Double;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fieldType));
            }
        }

        private static BufferUsageARB GetBufferUsageARB(UsageMode usageMode) {
            switch(usageMode)
            {
                case UsageMode.Static:
                    return BufferUsageARB.StaticDraw;
                case UsageMode.Dynamic:
                    return BufferUsageARB.DynamicDraw;
                case UsageMode.Stream:
                    return BufferUsageARB.StreamDraw;
                default:
                    throw new ArgumentOutOfRangeException(nameof(usageMode));
            }
        }

        private static PrimitiveType GetPrimitiveType(DrawMode drawMode)
        {
            switch (drawMode)
            {
                case DrawMode.Lines:
                    return PrimitiveType.Lines;
                case DrawMode.LineLoop:
                    return PrimitiveType.LineLoop;
                case DrawMode.LineStrip:
                    return PrimitiveType.LineStrip;
                case DrawMode.Triangles:
                    return PrimitiveType.Triangles;
                case DrawMode.TriangleStrip:
                    return PrimitiveType.TriangleStrip;
                case DrawMode.TriangleFan:
                    return PrimitiveType.TriangleFan;
                case DrawMode.Quads:
                    return PrimitiveType.Quads;
                default:
                    throw new ArgumentOutOfRangeException(nameof(drawMode));
            }
        }
    }
}