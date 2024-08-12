﻿using AvionEngine.Interfaces;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using System.Drawing;
using Silk.NET.Maths;
using AvionEngine.Enums;

namespace AvionEngine.OpenGL
{
    public class Renderer : IRenderer
    {
        public IWindow Window { get; }
        public Color ClearColor 
        { 
            get => clearColor;
            set
            {
                glInstance.ClearColor(value);
                clearColor = value;
            }
        }
        private GL glInstance;
        private Color clearColor = Color.Black;

        public Renderer(IWindow window)
        {
            Window = window;
            glInstance = window.CreateOpenGL();
        }

        public IShader CreateShader(string vertex, string fragment)
        {
            return new Rendering.Shader(glInstance, vertex, fragment);
        }

        public IMesh CreateMesh<TVertex>(TVertex[] vertices, uint[] indices, UsageMode usageMode = UsageMode.Static, DrawMode drawMode = DrawMode.Triangles) where TVertex : unmanaged
        {
            var mesh = new Rendering.Mesh(glInstance, usageMode);
            mesh.Update(vertices, indices);
            return mesh;
        }

        public ITexture CreateTexture(
            uint width, 
            uint height, 
            byte[] data, 
            uint depth = 0, 
            TextureTargetMode targetMode = TextureTargetMode.Texture2D, 
            TextureFormatMode formatMode = TextureFormatMode.RGB,
            WrapMode wrapModeS = WrapMode.Repeat,
            WrapMode wrapModeT = WrapMode.Repeat,
            WrapMode wrapModeR = WrapMode.Repeat,
            MinFilterMode minFilterMode = MinFilterMode.Linear,
            MagFilterMode magFilterMode = MagFilterMode.Linear)
        {
            return new Rendering.Texture(glInstance, width, height, data, depth, targetMode, formatMode, wrapModeS, wrapModeT, wrapModeR, minFilterMode, magFilterMode);
        }

        public void Resize(Vector2D<int> newSize)
        {
            glInstance.Viewport(newSize);
        }

        public void Clear()
        {
            glInstance.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
        //glInstance.DrawElements(PrimitiveType.Triangles, indicesLength, DrawElementsType.UnsignedInt, (void*)0);
    }
}
