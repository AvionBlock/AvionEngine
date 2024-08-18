using AvionEngine.Interfaces;
using AvionEngine.Rendering;

namespace Tester.Components
{
    public struct MeshComponent
    {
        public BaseMesh Mesh { get; set; }
        public List<IVisual> Materials { get; set; }
        public BaseTexture? Texture { get; set; }

        public MeshComponent(BaseMesh mesh)
        {
            Mesh = mesh;
            Materials = new List<IVisual>();
        }

        public void Render(double delta)
        {
            Texture?.Render(delta);
            for (int i = 0; i < Materials?.Count; i++)
            {
                Materials[i].Render(delta);
            }
            Mesh.Render(delta);
        }
    }
}