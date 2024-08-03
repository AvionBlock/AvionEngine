using AvionEngine.Interfaces;
using AvionEngine.Rendering;

namespace Tester.Components
{
    public struct MeshComponent<TVertex> where TVertex : unmanaged
    {
        public BaseMesh<TVertex> Mesh { get; set; }
        public List<IVisual> Materials { get; set; }

        public MeshComponent(BaseMesh<TVertex> mesh)
        {
            Mesh = mesh;
            Materials = new List<IVisual>();
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