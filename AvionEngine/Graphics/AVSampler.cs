using System;
using System.Numerics;

namespace AvionEngine.Graphics
{
    public abstract class AVSampler : IDisposable
    {
        public TextureFilterMode TextureMinFilterMode;
        public TextureFilterMode TextureMagFilterMode;
        public TextureFilterMode TextureMipFilterMode;
        public TextureWrapMode TextureWrapModeS;
        public TextureWrapMode TextureWrapModeT;
        public TextureWrapMode TextureWrapModeR;
        public Vector4 BorderColor;

        public bool IsDisposed { get; protected set; }

        public AVSampler(
            TextureFilterMode textureMinFilterMode,
            TextureFilterMode textureMagFilterMode,
            TextureFilterMode textureMipFilterMode,
            TextureWrapMode textureWrapModeS,
            TextureWrapMode textureWrapModeT,
            TextureWrapMode textureWrapModeR,
            Vector4 borderColor)
        {
            TextureMinFilterMode = textureMinFilterMode;
            TextureMagFilterMode = textureMagFilterMode;
            TextureMipFilterMode = textureMipFilterMode;
            TextureWrapModeS = textureWrapModeS;
            TextureWrapModeT = textureWrapModeT;
            TextureWrapModeR = textureWrapModeR;
            BorderColor = borderColor;
        }

        public virtual void SetFilter(TextureFilterMode? textureMinFilterMode = null, TextureFilterMode? textureMagFilterMode = null, TextureFilterMode? textureMipFilterMode = null)
        {
            TextureMinFilterMode = textureMinFilterMode ?? TextureMinFilterMode;
            TextureMagFilterMode = textureMagFilterMode ?? TextureMagFilterMode;
            TextureMipFilterMode = textureMipFilterMode ?? TextureMipFilterMode;
        }

        public virtual void SetWrap(TextureWrapMode? textureWrapModeS = null, TextureWrapMode? textureWrapModeT = null, TextureWrapMode? textureWrapModeR = null)
        {
            TextureWrapModeS = textureWrapModeS ?? TextureWrapModeS;
            TextureWrapModeT = textureWrapModeT ?? TextureWrapModeT;
            TextureWrapModeR = textureWrapModeR ?? TextureWrapModeR;
        }

        public virtual void SetBorderColor(Vector4? borderColor = null)
        {
            BorderColor = borderColor ?? BorderColor;
        }

        public abstract void Dispose();
    }
}
