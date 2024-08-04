using AvionEngine.Interfaces;

namespace AvionEngine.Rendering
{
    public class BaseMesh : IRenderable
    {
        private IMesh nativeMesh;
        public virtual IMesh NativeMesh
        { 
            get => nativeMesh;
            set => nativeMesh = value;
        } //We can swap out native mesh if we need to. Give the option to the user to set or ignore setting the NativeMesh.

        public BaseMesh(IMesh nativeMesh)
        {
            this.nativeMesh = nativeMesh;
        }

        public virtual void Set<TVertex>(TVertex[] vertices, uint[] indices) where TVertex : unmanaged
        {
            NativeMesh.Set(vertices, indices);
        }

        public virtual void Render(double delta)
        {
            NativeMesh.Render(delta);
        }
    }
}
