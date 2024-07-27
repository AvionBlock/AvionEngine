using AvionEngine.Interfaces;

namespace AvionEngine.Rendering
{
    public abstract class BaseMaterial : IVisual
    {
        private IVisual Visual { get; set; }

        public BaseMaterial(IVisual visual)
        {
            Visual = visual;
        }

        public virtual void Render(double delta)
        {
            Visual.Render(delta);
        }
    }
}