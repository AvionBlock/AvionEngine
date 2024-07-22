using AvionEngine.Interfaces;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using System.Drawing;
using AvionEngine.Rendering;
using System.Collections.Generic;

namespace AvionEngine.D3D12
{
    public class Renderer : IRenderer
    {
        public IWindow Window { get; }
        public Color ClearColor { get; set; } = Color.Black;
        private GL glInstance;

        public Renderer(IWindow window)
        {
            Window = window;
            glInstance = window.CreateOpenGL();
        }

        public IShader CreateShader(BaseShader baseShader)
        {
            return new OpenGL.Rendering.Shader(glInstance, baseShader);
        }

        public void Clear()
        {
            glInstance.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        //glInstance.DrawElements(PrimitiveType.Triangles, indicesLength, DrawElementsType.UnsignedInt, (void*)0);
    }
}
