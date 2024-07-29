using AvionEngine.Interfaces;
using AvionEngine.Rendering;

namespace Tester
{
    public class ProjectionShader : BaseShader
    {
        public ProjectionShader(IRenderer renderer, string vertex, string fragment) : base(renderer, vertex, fragment)
        {
        }
    }
}
