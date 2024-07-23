using AvionEngine.Interfaces;
using AvionEngine.Rendering;
using System.Collections.Generic;

namespace AvionEngine
{
    public class AvionEngine : IEngine
    {
        public IRenderer Renderer { get; private set; } //We can swap out rendering engines.
        public IEnumerable<BaseShader> Shaders { get => shaders; }
        public IScene Scene { get; set; } //Changeable scenes.

        public List<BaseShader> shaders { get; set; } = new List<BaseShader>();

        public AvionEngine(IRenderer renderer, IScene scene)
        {
            Renderer = renderer; //Set the renderer the user wants first.
            Scene = scene;

            renderer.Window.Update += Update;
            renderer.Window.Render += Render;
        }

        public void AddShader(BaseShader baseShader)
        {
            Renderer.CreateShader(baseShader);
            shaders.Add(baseShader);
        }

        public void RemoveShader(BaseShader baseShader)
        {
            shaders.Remove(baseShader);
        }

        public void SetRenderer(IRenderer renderer)
        {
            Renderer.Window.Update -= Update;
            Renderer.Window.Render -= Render;

            Renderer = renderer;

            Renderer.Window.Update += Update;
            Renderer.Window.Render += Render;

            foreach(var shader in shaders)
            {
                Renderer.CreateShader(shader);
            }
            //NOT FINISHED!
        }

        private void Update(double delta)
        {
            Renderer.Clear();
            for (int i = 0; i < Scene.EngineObjects.Count; i++)
            {
                foreach (var component in Scene.EngineObjects[i].Components)
                {
                    component.Update(delta);
                }
            }
        }

        private void Render(double delta)
        {
            for (int i = 0; i < Scene.EngineObjects.Count; i++)
            {
                foreach (var component in Scene.EngineObjects[i].Components)
                {
                    component.Render(delta);
                }
            }
        }
    }
}
