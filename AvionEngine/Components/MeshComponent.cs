using AvionEngine.Interfaces;
using AvionEngine.Rendering;
using System.Collections.Generic;

namespace AvionEngine.Components
{
    public struct MeshComponent
    {
        public BaseMesh Mesh { get; set; }
        public List<IVisual> Materials { get; set; }

        public void Set<T>(T[] vertices, uint[] indices) where T : unmanaged
        {
            Mesh.Set(vertices, indices);
        }

        public void Render(double delta)
        {
            for (int i = 0; i < Materials?.Count; i++)
            {
                Materials[i].Render(delta);
            }
            Mesh.Render(delta);
        }
    }
}