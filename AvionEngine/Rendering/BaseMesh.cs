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
