using Silk.NET.Windowing;
using Silk.NET.OpenGL;

namespace AvionEngine.OpenGL
{
    public class Renderer : AVRenderer
    {
        public readonly GL glContext;

        public Renderer(IWindow window) : base(window)
        {
            glContext = window.CreateOpenGL();
        }
    }
}
