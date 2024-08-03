using AvionEngine.Interfaces;
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

        public ITexture CreateTexture2D(uint width, uint height, byte[] data, TextureFormat format = TextureFormat.RGB)
        {
            return new Rendering.Texture2D(glInstance, width, height, data, format);
        }

        public IMesh<TVertex> CreateMesh<TVertex>(TVertex[] vertices, uint[] indices, DrawMode drawMode = DrawMode.Static) where TVertex : unmanaged
        {
            return new Rendering.Mesh<TVertex>(glInstance, vertices, indices, drawMode);
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
