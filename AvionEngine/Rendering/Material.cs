using AvionEngine.Interfaces;

namespace AvionEngine.Rendering
{
    public class Material : IRenderable
    {
        private IRenderable Renderable { get; set; }

        public Material(IRenderable renderable)
        {
            Renderable = renderable;
        }

        public virtual void Render()
        {
            Renderable.Render();
        }
    }
}