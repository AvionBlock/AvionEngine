using AvionEngine.Interfaces;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using System.Drawing;

namespace AvionEngine.D3D12
{
    public class Renderer : IRenderer
    {
        public Color ClearColor { get; set; } = Color.Black;
        public IWindow Window { get; }

        private GL glInstance;

        public Renderer(IWindow window)
        {
            Window = window;
            glInstance = window.CreateOpenGL();
        }

        public IShader CreateShader(string vertexCode, string fragmentCode)
        {
            return new OpenGL.Rendering.Shader(glInstance, vertexCode, fragmentCode);
        }

        public unsafe void Draw(uint indicesLength)
        {
            glInstance.DrawElements(PrimitiveType.Triangles, indicesLength, DrawElementsType.UnsignedInt, (void*)0);
        }

        public void Clear()
        {
            glInstance.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
    }
}
