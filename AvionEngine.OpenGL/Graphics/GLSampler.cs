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
            Vector4 borderColor) : base(
                textureMinFilterMode,
                textureMagFilterMode,
                textureMipFilterMode,
                textureWrapModeS,
                textureWrapModeT,
                textureWrapModeR,
                borderColor)
        {
            Renderer = renderer;

            Sampler = renderer.glContext.CreateSampler();
            SetFilter(null, null, null);
            SetWrap(null, null, null);
            SetBorderColor(null);
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

        public override void Dispose()
        {
            throw new NotImplementedException();
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
    }
}
