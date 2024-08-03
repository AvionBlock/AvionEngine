using AvionEngine.Enums;
using AvionEngine.Interfaces;

namespace AvionEngine.Rendering
{
    public class BaseMesh<TVertex> : IRenderable where TVertex : unmanaged
    {
        private IMesh<TVertex> nativeMesh;
        public virtual IMesh<TVertex> NativeMesh
        { 
            get => nativeMesh;
            set => nativeMesh = value;
        } //We can swap out native mesh if we need to. Give the option to the user to set or ignore setting the NativeMesh.

        public BaseMesh(IRenderer renderer, TVertex[] vertices, uint[] indices, DrawMode drawMode = DrawMode.Static)
        {
            nativeMesh = renderer.CreateMesh<TVertex>(vertices, indices, drawMode);
        }

        public BaseMesh(IMesh<TVertex> nativeMesh)
        {
            this.nativeMesh = nativeMesh;
        }

        public virtual void Set(TVertex[] vertices, uint[] indices)
        {
            NativeMesh.Set(vertices, indices);
        }

        public virtual void Render(double delta)
        {
            NativeMesh.Render(delta);
        }
    }
}
