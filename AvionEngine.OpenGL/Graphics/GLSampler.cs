using AvionEngine.Graphics;
using Silk.NET.OpenGL;
using System;
using System.Numerics;

namespace AvionEngine.OpenGL.Graphics
{
    public class GLSampler : AVSampler
    {
        public readonly GLRenderer Renderer;
        public readonly uint Sampler;

        public unsafe GLSampler(
            GLRenderer renderer,
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
            float maxLod) : base(
                textureMinFilterMode,
                textureMagFilterMode,
                textureMipFilterMode,
                textureWrapModeS,
                textureWrapModeT,
                textureWrapModeR,
                comparisonFunction,
                borderColor,
                mipLodBias,
                minLod,
                maxLod)
        {
            Renderer = renderer;

            Sampler = renderer.glContext.CreateSampler();
            SetFilter();
            SetWrap();
            SetBorderColor();
            SetLod();
        }

        public override void SetFilter(TextureFilterMode? textureMinFilterMode = null, TextureFilterMode? textureMagFilterMode = null, TextureFilterMode? textureMipFilterMode = null)
        {
            Renderer.glContext.SamplerParameter(Sampler, SamplerParameterI.MinFilter, (int)GetTextureMinFilter(TextureMinFilterMode, TextureMipFilterMode));
            Renderer.glContext.SamplerParameter(Sampler, SamplerParameterI.MagFilter, (int)GetTextureMagFilter(TextureMagFilterMode));
        }

        public override void SetWrap(TextureWrapMode? textureWrapModeS = null, TextureWrapMode? textureWrapModeT = null, TextureWrapMode? textureWrapModeR = null)
        {
            base.SetWrap(textureWrapModeS, textureWrapModeT, textureWrapModeR);

            Renderer.glContext.SamplerParameter(Sampler, SamplerParameterI.WrapS, (int)GetTextureWrapMode(TextureWrapModeS));
            Renderer.glContext.SamplerParameter(Sampler, SamplerParameterI.WrapT, (int)GetTextureWrapMode(TextureWrapModeT));
            Renderer.glContext.SamplerParameter(Sampler, SamplerParameterI.WrapR, (int)GetTextureWrapMode(TextureWrapModeR));
        }

        public override unsafe void SetBorderColor(Vector4? borderColor = null)
        {
            base.SetBorderColor(borderColor);

            var bColor = BorderColor;
            Renderer.glContext.SamplerParameter(Sampler, SamplerParameterF.BorderColor, &bColor.X);
        }

        public override void SetCompare(ComparisonFunction? comparisonFunction = null)
        {
            base.SetCompare(comparisonFunction);

            Renderer.glContext.SamplerParameter(Sampler, SamplerParameterI.CompareFunc, (int)GetDepthFunction(ComparisonFunction));
        }

        public override void SetLod(float? mipLodBias = null, float? minLod = null, float? maxLod = null)
        {
            base.SetLod(mipLodBias, minLod, maxLod);

            Renderer.glContext.SamplerParameter(Sampler, SamplerParameterF.LodBias, MipLodBias);
            Renderer.glContext.SamplerParameter(Sampler, SamplerParameterF.MinLod, MinLod);
            Renderer.glContext.SamplerParameter(Sampler, SamplerParameterF.MaxLod, MaxLod);
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                Renderer.glContext.DeleteSampler(Sampler);
            }

            IsDisposed = true;
        }

        ~GLSampler()
        {
            Dispose(false);
        }

        public static TextureMinFilter GetTextureMinFilter(TextureFilterMode textureMinFilterMode, TextureFilterMode textureMipFilterMode)
        {
            return (textureMinFilterMode, textureMipFilterMode) switch
            {
                (TextureFilterMode.Point, TextureFilterMode.Point) => TextureMinFilter.NearestMipmapNearest,
                (TextureFilterMode.Point, TextureFilterMode.Linear) => TextureMinFilter.NearestMipmapLinear,
                (TextureFilterMode.Linear, TextureFilterMode.Point) => TextureMinFilter.LinearMipmapNearest,
                (TextureFilterMode.Linear, TextureFilterMode.Linear) => TextureMinFilter.LinearMipmapLinear,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static TextureMagFilter GetTextureMagFilter(TextureFilterMode textureMagFilterMode)
        {
            return textureMagFilterMode switch
            {
                TextureFilterMode.Point => TextureMagFilter.Nearest,
                TextureFilterMode.Linear => TextureMagFilter.Linear,
                _ => throw new ArgumentOutOfRangeException(nameof(textureMagFilterMode))
            };
        }

        public static Silk.NET.OpenGL.TextureWrapMode GetTextureWrapMode(TextureWrapMode textureWrapMode)
        {
            return textureWrapMode switch
            {
                TextureWrapMode.Repeat => Silk.NET.OpenGL.TextureWrapMode.Repeat,
                TextureWrapMode.RepeatMirrored => Silk.NET.OpenGL.TextureWrapMode.MirroredRepeat,
                TextureWrapMode.ClampToEdge => Silk.NET.OpenGL.TextureWrapMode.ClampToEdge,
                TextureWrapMode.ClampToBorder => Silk.NET.OpenGL.TextureWrapMode.ClampToBorder,
                _ => throw new ArgumentOutOfRangeException(nameof(textureWrapMode))
            };
        }

        public static DepthFunction GetDepthFunction(ComparisonFunction comparisonMode)
        {
            return comparisonMode switch
            {
                ComparisonFunction.Never => DepthFunction.Never,
                ComparisonFunction.Less => DepthFunction.Less,
                ComparisonFunction.Equal => DepthFunction.Equal,
                ComparisonFunction.LessEqual => DepthFunction.Lequal,
                ComparisonFunction.Greater => DepthFunction.Greater,
                ComparisonFunction.NotEqual => DepthFunction.Notequal,
                ComparisonFunction.GreaterEqual => DepthFunction.Gequal,
                ComparisonFunction.Always => DepthFunction.Always,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
