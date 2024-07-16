using AvionEngine.Interfaces;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using System.Drawing;
using System.Collections.Generic;

namespace AvionEngine.D3D12
{
    public class Renderer : IRenderer
    {
        public Color ClearColor { get; set; } = Color.Black;
        public List<IShader> Shaders { get; set; }
        public List<EngineObject> Objects { get; set; }

        private IWindow window;
        private GL glInstance;

        public Renderer(IWindow window)
        {
            this.window = window;
            glInstance = window.CreateOpenGL();
            Shaders = new List<IShader>();
            Objects = new List<EngineObject>();

            //Event Registries
            window.Update += OnUpdate;
        }

        public IShader CreateShader(string vertexCode, string fragmentCode)
        {
            return new OpenGL.Rendering.Shader(glInstance, vertexCode, fragmentCode);
        }

        public unsafe void Draw(uint indicesLength)
        {
            glInstance.DrawElements(PrimitiveType.Triangles, indicesLength, DrawElementsType.UnsignedInt, (void*)0);
        }

        private void OnUpdate(double obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
