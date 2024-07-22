using AvionEngine.Structures;
using System.Collections.Generic;

namespace AvionEngine
{
    public class EngineObject
    {
        public Transform<float, float, float> Transform { get; set; } = new Transform<float, float, float>();
        public IEngine Engine { get; set; }

        private List<Component> components = new List<Component>();

        public EngineObject(IEngine engine)
        {
            Engine = engine;
        }

        public void AddComponent(Component component)
        {
            components.Add(component);
            component.Object = this;
        }

        public void RemoveComponent(Component component)
        {
            components.Remove(component);
        }

        public T? GetComponent<T>() where T : Component
        {
            for(int i = 0; i < components.Count; i++)
            {
                if (components[i] is T)
                {
                    return (T)components[i];
                }
            }
            return null;
        }
    }
}
