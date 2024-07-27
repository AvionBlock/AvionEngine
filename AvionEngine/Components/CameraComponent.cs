using AvionEngine.Interfaces;
using AvionEngine.Rendering;

namespace AvionEngine.Components
{
    public struct CameraComponent : IRenderable
    {
        public BaseShader ProjectionShader { get; set; }

        public void Render(double delta)
        {
            ProjectionShader.Render(delta);
        }
    }
}
