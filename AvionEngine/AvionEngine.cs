using AvionEngine.Interfaces;
using AvionEngine.Structures;
using System;
using System.Collections.Generic;

namespace AvionEngine
{
    public class AvionEngine : IEngine
    {
        public IRenderer Renderer { get; private set; } //We can swap out rendering engines.
        public List<EngineObject> EngineObjects { get; set; }
        public List<Type> ComponentSystems { get; set; }

        public AvionEngine(IRenderer renderer)
        {
            EngineObjects = new List<EngineObject>();
            ComponentSystems = new List<Type>();
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

        public virtual void Update(double delta)
        {
            Renderer.Clear();
            foreach (var type in ComponentSystems)
            {
                var methodInfo = type.GetMethod(nameof(BaseSystem<Component>.Update));

                if (methodInfo != null && methodInfo.IsStatic)
                {
                    methodInfo.Invoke(null, new object[] { delta });
                }
            }
        }

        private void Render(double delta)
        {
            foreach (var type in ComponentSystems)
            {
                var methodInfo = type.GetMethod(nameof(BaseSystem<Component>.Render));

                if (methodInfo != null && methodInfo.IsStatic)
                {
                    methodInfo.Invoke(null, new object[] { delta });
                }
            }
        }
    }
}
