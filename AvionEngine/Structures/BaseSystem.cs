using System.Collections.Generic;

namespace AvionEngine.Structures
{
    public class BaseSystem<T> where T : Component
    {
        protected static List<T> components = new List<T>();

        public static void Register(T component)
        {
            components.Add(component);
        }

        public static void Unregister(T component)
        {
            components.Remove(component);
        }

        public static void Update(double delta)
        {
            foreach(var component in components)
            {
                component.Update(delta);
            }
        }

        public static void Render(double delta)
        {
            foreach (var component in components)
            {
                component.Render(delta);
            }
        }
    }
}
