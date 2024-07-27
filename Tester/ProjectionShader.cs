using AvionEngine.Interfaces;
using AvionEngine.Rendering;
using Silk.NET.Maths;

namespace Tester
{
    public class ProjectionShader : BaseShader
    {
        public ProjectionShader(IRenderer renderer, string vertex, string fragment) : base(renderer, vertex, fragment)
        {
        }

        public override void Render(double delta)
        {
            base.Render(delta);
            var model = Matrix4X4<float>.Identity; //Caching is faster than storing in a property.

            NativeShader.SetUniform4(nameof(model), model);
            //NativeShader.SetUniform4(nameof(view), view);
            //NativeShader.SetUniform4(nameof(projection), projection);
        }
    }
}
