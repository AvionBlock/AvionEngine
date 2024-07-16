using AvionEngine.Rendering;
using AvionEngine.Structures;
using System.Collections.Generic;

namespace AvionEngine.Components
{
    public class MeshComponent : Component
    {
        public uint[] Indices { get; set; }
        public List<Material> Materials { get; set; }
        //Create Mesh Here.

        public MeshComponent()
        {
            Indices = new uint[0];
            Materials = new List<Material>();
        }

        public void Set(Vertex[] vertices, uint[] indices)
        {
            //Set mesh here.
        }
    }
}
