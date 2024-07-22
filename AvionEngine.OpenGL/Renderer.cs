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
        public IEnumerable<IShader> Shaders { get => shaders; }

        private List<IShader> shaders = new List<IShader>();
        private GL glInstance;

        public Renderer(IWindow window)
        {
            Window = window;
            glInstance = window.CreateOpenGL();
        }

        public void AddShader(BaseShader baseShader)
        {
            shaders.Add(new OpenGL.Rendering.Shader(glInstance, baseShader));
        }

        public bool RemoveShader(BaseShader baseShader)
        {
            var shader = shaders.Find(x => x.BaseShader == baseShader);
            if (shader != null)
            {
                shaders.Remove(shader);
                return true;
            }
            return false;
        }

        public void ClearShaders()
        {
            shaders.Clear();
        }

        public void Clear()
        {
            glInstance.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        //glInstance.DrawElements(PrimitiveType.Triangles, indicesLength, DrawElementsType.UnsignedInt, (void*)0);
    }
}
