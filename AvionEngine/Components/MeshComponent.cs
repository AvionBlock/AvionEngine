using AvionEngine.Interfaces;
using AvionEngine.Rendering;
using AvionEngine.Structures;
using System.Collections.Generic;

namespace AvionEngine.Components
{
    public struct MeshComponent : IRenderable
    {
        public BaseMesh Mesh { get; set; }
        public List<IRenderable> Materials { get; set; }

        public void Set(Vertex[] vertices, uint[] indices)
        {
            Mesh.Set(vertices, indices);
        }

        public void Render(double delta)
        {
            foreach (var m in Materials)
            {
                m.Render(delta);
            }
        }
    }
}
