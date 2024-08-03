using AvionEngine.Interfaces;
using Silk.NET.OpenGL;
using System.Runtime.InteropServices;
using System;
using System.Reflection;
using AvionEngine.Structures;
using AvionEngine.Enums;

namespace AvionEngine.OpenGL.Rendering
{
    public class Mesh<TVertex> : IMesh<TVertex> where TVertex : unmanaged
    {
        private GL glInstance;
        private uint VBO;
        private uint VAO;
        private uint EBO;
        private uint indicesLength;
        private bool disposed;

        public bool IsDisposed { get => disposed; }

        public unsafe Mesh(GL glInstance, TVertex[] vertices, uint[] indices, DrawMode drawMode = DrawMode.Static)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(Mesh<TVertex>));

            this.glInstance = glInstance;

            //Create Buffers and Arrays
            VAO = glInstance.GenVertexArray();
            VBO = glInstance.GenBuffer();
            EBO = glInstance.GenBuffer();

            glInstance.BindVertexArray(VAO);
            var verticeFields = typeof(TVertex).GetFields();

            //Load data
            glInstance.BindBuffer(BufferTargetARB.ArrayBuffer, VBO);
            fixed (void* verticesPtr = vertices)
                glInstance.BufferData(BufferTargetARB.ArrayBuffer, (UIntPtr)(vertices.Length * sizeof(TVertex)), verticesPtr, GetBufferUsageARB(drawMode));

            glInstance.BindBuffer(BufferTargetARB.ElementArrayBuffer, EBO);
            fixed (uint* indicesPtr = indices)
                glInstance.BufferData(BufferTargetARB.ElementArrayBuffer, (UIntPtr)(indices.Length * sizeof(uint)), indicesPtr, GetBufferUsageARB(drawMode));

            for (uint i = 0; i < verticeFields.Length; i++)
            {
                var fieldSize = verticeFields[i].FieldType.GetFields().Length;
                if (fieldSize <= 0)
                    fieldSize = 1;

                glInstance.EnableVertexAttribArray(i);
                glInstance.VertexAttribPointer(
                    i,
                    fieldSize,
                    GetVertexAttribPointerType(verticeFields[i].GetCustomAttribute<VertexFieldType>().FieldType),
                    false,
                    (uint)sizeof(TVertex),
                    (void*)Marshal.OffsetOf<TVertex>(verticeFields[i].Name));
            }

            glInstance.BindVertexArray(0);
            glInstance.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
            glInstance.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);
            indicesLength = (uint)indices.Length;
        }

        public unsafe void Set(TVertex[] vertices, uint[] indices, int offset = 0)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(Mesh<TVertex>));

            glInstance.BindVertexArray(VAO);

            //Load data
            glInstance.BindBuffer(BufferTargetARB.ArrayBuffer, VBO);
            fixed (void* verticesPtr = vertices)
                glInstance.BufferSubData(BufferTargetARB.ArrayBuffer, offset, (UIntPtr)(vertices.Length * sizeof(TVertex)), verticesPtr);

            glInstance.BindBuffer(BufferTargetARB.ElementArrayBuffer, EBO);
            fixed (uint* indicesPtr = indices)
                glInstance.BufferSubData(BufferTargetARB.ElementArrayBuffer, offset, (UIntPtr)(indices.Length * sizeof(uint)), indicesPtr);

            glInstance.BindVertexArray(0);
            glInstance.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
            glInstance.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);
            indicesLength = (uint)indices.Length;
        }

        public unsafe void Render(double delta)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(Mesh<TVertex>));

            glInstance.BindVertexArray(VAO);
            glInstance.DrawElements(PrimitiveType.Triangles, indicesLength, DrawElementsType.UnsignedInt, (void*)0);
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

        private static VertexAttribPointerType GetVertexAttribPointerType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return VertexAttribPointerType.Byte;
                case TypeCode.Byte:
                    return VertexAttribPointerType.UnsignedByte;
                case TypeCode.Int16:
                    return VertexAttribPointerType.Short;
                case TypeCode.UInt16:
                    return VertexAttribPointerType.UnsignedShort;
                case TypeCode.Int32:
                    return VertexAttribPointerType.Int;
                case TypeCode.UInt32:
                    return VertexAttribPointerType.UnsignedInt;
                case TypeCode.Double:
                    return VertexAttribPointerType.Double;

                    //I have no idea wtf these are.
                case TypeCode.Int64:
                    return VertexAttribPointerType.Int64Arb;
                case TypeCode.UInt64:
                    return VertexAttribPointerType.UnsignedInt64Arb;

                default:
                    return VertexAttribPointerType.Float;
            }
        }

        private static BufferUsageARB GetBufferUsageARB(DrawMode mode) {
            switch(mode)
            {
                case DrawMode.Dynamic:
                    return BufferUsageARB.DynamicDraw;
                case DrawMode.Stream:
                    return BufferUsageARB.StreamDraw;
                default:
                    return BufferUsageARB.StaticDraw;
            }
        }
    }
}