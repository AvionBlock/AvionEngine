using AvionEngine.Interfaces;
using AvionEngine.Rendering;
using System.Collections.Generic;

namespace AvionEngine
{
    public class AvionEngine : IEngine
    {
        public IRenderer Renderer { get; private set; } //We can swap out rendering engines.
        public List<Shader> Shaders { get; set; }
        public List<EngineObject> EnginObjects { get; set; }

        public AvionEngine(IRenderer renderer)
        {
            Shaders = new List<Shader>();
            EnginObjects = new List<EngineObject>();
            Renderer = renderer; //Set the renderer the user wants first.

            renderer.Window.Update += Update;
            renderer.Window.Render += Render;
        }

        public void SetRenderer(IRenderer renderer)
        {
            Renderer.Window.Update -= Update;
            Renderer.Window.Render -= Render;

            Renderer = renderer;

            Renderer.Window.Update += Update;
            Renderer.Window.Render += Render;

            //NOT FINISHED!
        }

        public virtual void Update(double obj)
        {
            Renderer.Clear();
        }

        private void Render(double obj)
        {
            for (int i = 0; i < Shaders.Count; i++)
            {
                Shaders[i].Bind();
            }
        }
    }
}
