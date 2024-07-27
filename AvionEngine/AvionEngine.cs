using Arch.Core;
using AvionEngine.Interfaces;

namespace AvionEngine
{
    public class AvionEngine : IEngine
    {
        public IRenderer Renderer { get; private set; } //We can swap out rendering engines.
        public World World { get; private set; } //Swappable Worlds

        public AvionEngine(IRenderer renderer)
        {
            Renderer = renderer; //Set the renderer the user wants first.
            World = World.Create();

            renderer.Window.Update += Update;
        }

        public void SetRenderer(IRenderer renderer)
        {
            Renderer.Window.Update -= Update;

            Renderer = renderer;

            Renderer.Window.Update += Update;
            //NOT FINISHED!
        }

        private void Update(double delta)
        {
            Renderer.Clear();
        }
    }
}
