﻿using AvionEngine.Interfaces;
using AvionEngine.Structures;
using Silk.NET.OpenGL;
using System.Runtime.InteropServices;
using System;

namespace AvionEngine.OpenGL.Rendering
{
    public class Mesh : IMesh
    {
        public uint[] Indices { get; private set; }

        private GL glInstance;
        private uint VBO;
        private uint VAO;
        private uint EBO;

        public unsafe Mesh(GL glInstance, Vertex[] vertices, uint[] indices)
        {
            this.glInstance = glInstance;
            Indices = indices;

            //Create Buffers and Arrays
            VAO = glInstance.GenVertexArray();
            VBO = glInstance.GenBuffer();
            EBO = glInstance.GenBuffer();

            glInstance.BindVertexArray(VAO);
            glInstance.EnableVertexAttribArray(0);
            glInstance.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)0);

            glInstance.EnableVertexAttribArray(1);
            glInstance.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)Marshal.OffsetOf<Vertex>(nameof(Vertex.Normal)));

            glInstance.EnableVertexAttribArray(2);
            glInstance.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)Marshal.OffsetOf<Vertex>(nameof(Vertex.TexPosition)));
            Set(vertices, indices);
        }

        public unsafe void Set(Vertex[] vertices, uint[] indices)
        {
            glInstance.BindVertexArray(VAO);

            //Load data
            glInstance.BindBuffer(BufferTargetARB.ArrayBuffer, VBO);
            var vertexes = vertices.AsSpan();
            fixed (float* verticesPtr = MemoryMarshal.Cast<Vertex, float>(vertexes))
                glInstance.BufferData(BufferTargetARB.ArrayBuffer, (UIntPtr)(vertices.Length * sizeof(Vertex)), verticesPtr, BufferUsageARB.StaticDraw);

            glInstance.BindBuffer(BufferTargetARB.ElementArrayBuffer, EBO);
            fixed (uint* indicesPtr = indices)
                glInstance.BufferData(BufferTargetARB.ElementArrayBuffer, (UIntPtr)(indices.Length * sizeof(uint)), indicesPtr, BufferUsageARB.StaticDraw);

            glInstance.BindVertexArray(0);
            glInstance.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
            glInstance.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);
        }

        public unsafe void Render(double delta)
        {
            glInstance.BindVertexArray(VAO);
            glInstance.DrawElements(PrimitiveType.Triangles, (uint)Indices.Length, DrawElementsType.UnsignedInt, (void*)0);
            glInstance.BindVertexArray(0);
        }
    }
}
