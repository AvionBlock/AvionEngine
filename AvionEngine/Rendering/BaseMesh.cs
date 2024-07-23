using AvionEngine.Interfaces;
using AvionEngine.Structures;

namespace AvionEngine.Rendering
{
    public abstract class BaseMesh : IRenderable
    {
        protected IMesh? NativeMesh { get; set; } //We can swap out native mesh if we need to. Give the option to the user to set or ignore setting the NativeMesh.

        /// <summary>
        /// Called when a native mesh loads the base mesh.
        /// </summary>
        /// <param name="nativeMesh">The native mesh that loaded the base shader.</param>
        /// <param name="vertices">The vertices that the native mesh uploads to the GPU.</param>
        /// <param name="indices">The indices that the native mesh uploads to the GPU.</param>
        public abstract void Load(IMesh nativeMesh, out Vertex[] vertices, out uint[] indices);

        public virtual void Set(Vertex[] vertices, uint[] indices)
        {
            NativeMesh?.Set(vertices, indices);
        }

        public virtual void Render(double delta)
        {
            NativeMesh?.Render(delta);
        }
    }
}
