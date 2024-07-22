using AvionEngine.Interfaces;
using AvionEngine.Rendering;
using AvionEngine.Structures;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AvionEngine
{
    public class AvionEngine
    {
        public IRenderer Renderer { get; private set; } //We can swap out rendering engines.

        public IEnumerable<BaseShader> Shaders { get => shaders; }
        public IEnumerable<Type> ComponentSystems { get => componentSystems; }

        public List<EngineObject> EngineObjects { get; set; } = new List<EngineObject>();
        public List<BaseShader> shaders { get; set; } = new List<BaseShader>();
        public List<Type> componentSystems { get; set; } = new List<Type>();

        public AvionEngine(IRenderer renderer)
        {
            Renderer = renderer; //Set the renderer the user wants first.

            renderer.Window.Update += Update;
            renderer.Window.Render += Render;
        }

        public void AddShader(BaseShader baseShader)
        {
            shaders.Add(baseShader);
            Renderer.AddShader(baseShader);
        }

        public void RemoveShader(BaseShader baseShader)
        {
            shaders.Remove(baseShader);
            Renderer.RemoveShader(baseShader);
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

        private void Update(double delta)
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
