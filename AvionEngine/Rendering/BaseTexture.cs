using AvionEngine.Descriptors;
using AvionEngine.Enums;
using AvionEngine.Interfaces;
using System;

namespace AvionEngine.Rendering
{
    public abstract class BaseTexture : IVisual, IDisposable
    {
        public abstract IRenderer Renderer { get; }
        public bool IsDisposed { get; protected set; }

        public virtual void Render(double delta, int unit)
        {
            Assign(unit);
            Render(delta);
        }

        public abstract void Render(double delta);

        public abstract void Update(TextureDescriptor textureData, TextureTargetMode? targetMode = null, FormatType? formatMode = null);

        public abstract void Update(TextureDescriptor[] textureData, TextureTargetMode? targetMode = null, FormatType? formatMode = null);

        public abstract void UpdateWrapMode(WrapMode? wrapModeS = null, WrapMode? wrapModeT = null, WrapMode? wrapModeR = null);

        public abstract void UpdateFilterMode(MinFilterMode? minFilterMode = null, MagFilterMode? magFilterMode = null);

        public abstract void Assign(int unit = 0);

        public abstract void Dispose();
    }
}
