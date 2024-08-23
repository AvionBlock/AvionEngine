using AvionEngine.Enums;
using AvionEngine.Structures;
using System;

namespace AvionEngine.Interfaces
{
    public interface ITexture : IVisual, IDisposable
    {
        IRenderer Renderer { get; }

        bool IsDisposed { get; }

        void Update(TextureInfo textureData, TextureTargetMode? targetMode = null, TextureFormatMode? formatMode = null);

        void Update(TextureInfo[] textureData, TextureTargetMode? targetMode = null, TextureFormatMode? formatMode = null);

        void UpdateWrapMode(WrapMode? wrapModeS = null, WrapMode? wrapModeT = null, WrapMode? wrapModeR = null);

        void UpdateFilterMode(MinFilterMode? minFilterMode = null, MagFilterMode? magFilterMode = null);

        void Assign(int unit = 0);
    }
}
