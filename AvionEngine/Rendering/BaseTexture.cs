using AvionEngine.Interfaces;

namespace AvionEngine.Rendering
{
    public class BaseTexture : IVisual
    {
        private ITexture nativeTexture;
        public virtual ITexture NativeTexture
        {
            get => nativeTexture;
            set => nativeTexture = value;
        } //We can swap out native mesh if we need to. Give the option to the user to set or ignore setting the NativeMesh.

        public BaseTexture(ITexture nativeTexture)
        {
            this.nativeTexture = nativeTexture;
        }

        public void Render(double delta, int unit)
        {
            nativeTexture.Assign(unit);
            nativeTexture.Render(delta);
        }

        public void Render(double delta)
        {
            Render(delta, 0);
        }
    }
}
