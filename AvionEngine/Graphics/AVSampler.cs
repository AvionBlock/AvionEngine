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
        public ComparisonFunction ComparisonFunction;
        public Vector4 BorderColor;
        public float MipLodBias;
        public float MinLod;
        public float MaxLod;

        public bool IsDisposed { get; protected set; }

        public AVSampler(
            TextureFilterMode textureMinFilterMode,
            TextureFilterMode textureMagFilterMode,
            TextureFilterMode textureMipFilterMode,
            TextureWrapMode textureWrapModeS,
            TextureWrapMode textureWrapModeT,
            TextureWrapMode textureWrapModeR,
            ComparisonFunction comparisonFunction,
            Vector4 borderColor,
            float mipLodBias,
            float minLod,
            float maxLod)
        {
            TextureMinFilterMode = textureMinFilterMode;
            TextureMagFilterMode = textureMagFilterMode;
            TextureMipFilterMode = textureMipFilterMode;
            TextureWrapModeS = textureWrapModeS;
            TextureWrapModeT = textureWrapModeT;
            TextureWrapModeR = textureWrapModeR;
            ComparisonFunction = comparisonFunction;
            BorderColor = borderColor;
            MipLodBias = mipLodBias;
            MinLod = minLod;
            MaxLod = maxLod;
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

        public virtual void SetCompare(ComparisonFunction? comparisonFunction = null)
        {
            ComparisonFunction = comparisonFunction ?? ComparisonFunction;
        }

        public virtual void SetLod(float? mipLodBias = null, float? minLod = null, float? maxLod = null)
        {
            MipLodBias = mipLodBias ?? MipLodBias;
            MinLod = minLod ?? MinLod;
            MaxLod = maxLod ?? MaxLod;
        }

        public abstract void Dispose();
    }
}
