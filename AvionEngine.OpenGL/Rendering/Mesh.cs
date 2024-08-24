using AvionEngine.Interfaces;
using Silk.NET.OpenGL;
using System.Runtime.InteropServices;
using System;
using System.Reflection;
using AvionEngine.Structures;
using AvionEngine.Enums;
using System.Linq;
using AvionEngine.Rendering;

namespace AvionEngine.OpenGL.Rendering
{
    public class Mesh : BaseMesh
    {
        private readonly Renderer renderer;
        private readonly uint VBO;
        private readonly uint VAO;
        private readonly uint EBO;
        private uint indicesLength;
        private DrawMode drawMode;
        private UsageMode usageMode;
        private Type? vertexType;

        public override IRenderer Renderer { get => renderer; }

        public Mesh(Renderer renderer, UsageMode usageMode = UsageMode.Static, DrawMode drawMode = DrawMode.Triangles)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Mesh));

            this.renderer = renderer;
            this.usageMode = usageMode;
            this.drawMode = drawMode;

            //Create Buffers and Arrays
            VAO = this.renderer.glContext.GenVertexArray();
            VBO = this.renderer.glContext.GenBuffer();
            EBO = this.renderer.glContext.GenBuffer();
        }

        public override unsafe void Update<TVertex>(TVertex[] vertices, uint[] indices, UsageMode? usageMode = null, DrawMode? drawMode = null)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Mesh));
            this.usageMode = usageMode ?? this.usageMode;
            this.drawMode = drawMode ?? this.drawMode;

            renderer.glContext.BindVertexArray(VAO);

            //Load data
            renderer.glContext.BindBuffer(BufferTargetARB.ArrayBuffer, VBO);
            fixed (void* verticesPtr = vertices)
                renderer.glContext.BufferData(BufferTargetARB.ArrayBuffer, (UIntPtr)(vertices.Length * sizeof(TVertex)), verticesPtr, GetBufferUsageARB(this.usageMode));

            renderer.glContext.BindBuffer(BufferTargetARB.ElementArrayBuffer, EBO);
            fixed (uint* indicesPtr = indices)
                renderer.glContext.BufferData(BufferTargetARB.ElementArrayBuffer, (UIntPtr)(indices.Length * sizeof(uint)), indicesPtr, GetBufferUsageARB(this.usageMode));

            if(typeof(TVertex) != vertexType)
                UpdateVertexType<TVertex>();

            renderer.glContext.BindVertexArray(0);
            renderer.glContext.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
            renderer.glContext.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);
            indicesLength = (uint)indices.Length;
        }

        public override unsafe void UpdateVertexType<TVertex>()
        {
            var verticesFields = typeof(TVertex).GetFields().Where(x => Attribute.IsDefined(x, typeof(VertexField))).ToArray();

            for (uint i = 0; i < verticesFields.Length; i++)
            {
                var fieldSize = verticesFields[i].FieldType.GetFields().Length;
                if (fieldSize <= 0)
                    fieldSize = 1;

                renderer.glContext.EnableVertexAttribArray(i);
                renderer.glContext.VertexAttribPointer(
                    i,
                    fieldSize,
                    GetVertexAttribPointerType(verticesFields[i].GetCustomAttribute<VertexField>().FieldType),
                    false,
                    (uint)sizeof(TVertex),
                    (void*)Marshal.OffsetOf<TVertex>(verticesFields[i].Name));
            }

            vertexType = typeof(TVertex);
        }

        public override unsafe void Render(double delta)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Mesh));

            renderer.glContext.BindVertexArray(VAO);
            renderer.glContext.DrawElements(GetPrimitiveType(drawMode), indicesLength, DrawElementsType.UnsignedInt, (void*)0);
            renderer.glContext.BindVertexArray(0);
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
                renderer.glContext.DeleteVertexArray(VAO);
                renderer.glContext.DeleteBuffer(VBO);
                renderer.glContext.DeleteBuffer(EBO);
            }

            IsDisposed = true;
        }

        ~Mesh()
        {
            Dispose(false);
        }

        private static VertexAttribPointerType GetVertexAttribPointerType(FieldType fieldType)
        {
            return fieldType switch
            {
                FieldType.SByte => VertexAttribPointerType.Byte,
                FieldType.Byte => VertexAttribPointerType.UnsignedByte,
                FieldType.Int16 => VertexAttribPointerType.Short,
                FieldType.UInt16 => VertexAttribPointerType.UnsignedShort,
                FieldType.Int32 => VertexAttribPointerType.Int,
                FieldType.UInt32 => VertexAttribPointerType.UnsignedInt,
                FieldType.Int64 => VertexAttribPointerType.Int64Arb,
                FieldType.UInt64 => VertexAttribPointerType.UnsignedInt64Arb,
                FieldType.Single => VertexAttribPointerType.Float,
                FieldType.Double => VertexAttribPointerType.Double,
                _ => throw new ArgumentOutOfRangeException(nameof(fieldType))
            };
        }

        private static BufferUsageARB GetBufferUsageARB(UsageMode usageMode)
        {
            return usageMode switch
            {
                UsageMode.Static => BufferUsageARB.StaticDraw,
                UsageMode.Dynamic => BufferUsageARB.DynamicDraw,
                UsageMode.Stream => BufferUsageARB.StreamDraw,
                _ => throw new ArgumentOutOfRangeException(nameof(usageMode))
            };
        }

        private static PrimitiveType GetPrimitiveType(DrawMode drawMode)
        {
            return drawMode switch
            {
                DrawMode.Lines => PrimitiveType.Lines,
                DrawMode.LineLoop => PrimitiveType.LineLoop,
                DrawMode.LineStrip => PrimitiveType.LineStrip,
                DrawMode.Triangles => PrimitiveType.Triangles,
                DrawMode.TriangleStrip => PrimitiveType.TriangleStrip,
                DrawMode.TriangleFan => PrimitiveType.TriangleFan,
                DrawMode.Quads => PrimitiveType.Quads,
                _ => throw new ArgumentOutOfRangeException(nameof(drawMode))
            };
        }
    }
}