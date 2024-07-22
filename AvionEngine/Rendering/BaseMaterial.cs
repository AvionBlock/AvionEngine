using AvionEngine.Interfaces;

namespace AvionEngine.Rendering
{
    public abstract class BaseMaterial : IRenderable
    {
        private IRenderable Renderable { get; set; }

        public BaseMaterial(IRenderable renderable)
        {
            Renderable = renderable;
        }

        public virtual void Render(double delta)
        {
            Renderable.Render(delta);
        }
    }
}