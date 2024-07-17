using AvionEngine.Interfaces;
using AvionEngine.Structures;
using System.Collections.Generic;

namespace AvionEngine.Components
{
    public class MeshComponent : Component
    {
        public uint[] Indices { get; set; }
        public List<IRenderable> Materials { get; set; }
        //Create Mesh Here.

        public MeshComponent()
        {
            Indices = new uint[0];
            Materials = new List<IRenderable>();
        }

        public void Set(Vertex[] vertices, uint[] indices)
        {
            //Set mesh here.
        }

        public override void Render(double delta)
        {
            foreach (var m in Materials)
            {
                m.Render();
            }
        }
    }
}
