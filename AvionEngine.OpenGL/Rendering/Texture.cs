using System;
using AvionEngine.Enums;
using AvionEngine.Interfaces;
using AvionEngine.Rendering;
using AvionEngine.Structures;
using Silk.NET.OpenGL;

namespace AvionEngine.OpenGL.Rendering
{
    public class Texture : BaseTexture
    {
        private readonly Renderer renderer;
        private readonly uint texture;
        private TextureTargetMode targetMode;
        private TextureFormatMode formatMode;
        private WrapMode wrapModeS;
        private WrapMode wrapModeT;
        private WrapMode wrapModeR;
        private MinFilterMode minFilterMode;
        private MagFilterMode magFilterMode;

        public override IRenderer Renderer { get => renderer; }

        public Texture(Renderer renderer,
            TextureInfo textureData,
            TextureTargetMode targetMode = TextureTargetMode.Texture2D,
            TextureFormatMode formatMode = TextureFormatMode.RGB,
            WrapMode wrapModeS = WrapMode.Repeat,
            WrapMode wrapModeT = WrapMode.Repeat,
            WrapMode wrapModeR = WrapMode.Repeat,
            MinFilterMode minFilterMode = MinFilterMode.Linear,
            MagFilterMode magFilterMode = MagFilterMode.Linear)
        {
            this.renderer = renderer;
            this.targetMode = targetMode;
            this.formatMode = formatMode;
            this.wrapModeS = wrapModeS;
            this.wrapModeT = wrapModeT;
            this.wrapModeR = wrapModeR;
            this.minFilterMode = minFilterMode;
            this.magFilterMode = magFilterMode;

            texture = renderer.glContext.GenTexture();
            Update(textureData);
        }

        public Texture(Renderer renderer,
            TextureInfo[] textureData,
            TextureTargetMode targetMode = TextureTargetMode.Texture2D,
            TextureFormatMode formatMode = TextureFormatMode.RGB,
            WrapMode wrapModeS = WrapMode.Repeat,
            WrapMode wrapModeT = WrapMode.Repeat,
            WrapMode wrapModeR = WrapMode.Repeat,
            MinFilterMode minFilterMode = MinFilterMode.Linear,
            MagFilterMode magFilterMode = MagFilterMode.Linear)
        {
            this.renderer = renderer;
            this.targetMode = targetMode;
            this.formatMode = formatMode;
            this.wrapModeS = wrapModeS;
            this.wrapModeT = wrapModeT;
            this.wrapModeR = wrapModeR;
            this.minFilterMode = minFilterMode;
            this.magFilterMode = magFilterMode;

            texture = renderer.glContext.GenTexture();
            Update(textureData);
        }

        public override unsafe void Update(TextureInfo textureData,
            TextureTargetMode? targetMode = null, TextureFormatMode? formatMode = null)
        {
            this.targetMode = targetMode ?? this.targetMode;
            this.formatMode = formatMode ?? this.formatMode;

            renderer.glContext.BindTexture(GetTextureTarget(this.targetMode), texture);
            fixed (byte* dataPtr = textureData.Data)
            {
                switch (this.targetMode)
                {
                    case TextureTargetMode.Texture1D:
                        renderer.glContext.TexImage1D(GetTextureTarget(this.targetMode), 0, GetInternalFormat(this.formatMode),
                            textureData.Width, 0, GetPixelFormat(this.formatMode), GLEnum.UnsignedByte, dataPtr);
                        break;
                    case TextureTargetMode.Texture2D:
                        renderer.glContext.TexImage2D(GetTextureTarget(this.targetMode), 0, GetInternalFormat(this.formatMode),
                            textureData.Width, textureData.Height, 0, GetPixelFormat(this.formatMode), GLEnum.UnsignedByte, dataPtr);
                        break;
                    case TextureTargetMode.Texture3D:
                        renderer.glContext.TexImage3D(GetTextureTarget(this.targetMode), 0, GetInternalFormat(this.formatMode),
                            textureData.Width, textureData.Height, textureData.Depth, 0, GetPixelFormat(this.formatMode), GLEnum.UnsignedByte, dataPtr);
                        break;
                    case TextureTargetMode.CubeMap:
                    default:
                        throw new InvalidOperationException(
                            "Cannot apply this texture update with the specified target mode.");
                }
            }

            renderer.glContext.GenerateMipmap(GetTextureTarget(this.targetMode));
            renderer.glContext.BindTexture(GetTextureTarget(this.targetMode), 0); //Unbind Texture.
        }

        public override unsafe void Update(TextureInfo[] textureData,
            TextureTargetMode? targetMode = null, TextureFormatMode? formatMode = null)
        {   
            this.targetMode = targetMode ?? this.targetMode;
            this.formatMode = formatMode ?? this.formatMode;

            renderer.glContext.BindTexture(GetTextureTarget(this.targetMode), texture);
            switch (this.targetMode)
            {
                case TextureTargetMode.CubeMap:
                    if (textureData.Length != 6)
                        throw new ArgumentOutOfRangeException(nameof(textureData), $"Must have 6 textures for a target mode of {this.targetMode}.");

                    for (var i = 0; i < textureData.Length; i++)
                    {
                        fixed (byte* dataPtr = textureData[i].Data)
                        {
                            renderer.glContext.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0,
                                GetInternalFormat(this.formatMode), textureData[i].Width, textureData[i].Height, 0,
                                GetPixelFormat(this.formatMode), GLEnum.UnsignedByte, dataPtr);
                        }
                    }
                    break;
                case TextureTargetMode.Texture1D:
                case TextureTargetMode.Texture2D:
                case TextureTargetMode.Texture3D:
                default:
                    throw new InvalidOperationException(
                        "Cannot apply this texture update with the specified target mode.");
            }

            renderer.glContext.GenerateMipmap(GetTextureTarget(this.targetMode));
            renderer.glContext.BindTexture(GetTextureTarget(this.targetMode), 0); //Unbind Texture.
        }

        public override void UpdateWrapMode(WrapMode? wrapModeS = null, WrapMode? wrapModeT = null, WrapMode? wrapModeR = null)
        {
            this.wrapModeS = wrapModeS ?? this.wrapModeS;
            this.wrapModeT = wrapModeT ?? this.wrapModeT;
            this.wrapModeR = wrapModeR ?? this.wrapModeR;

            renderer.glContext.BindTexture(GetTextureTarget(targetMode), texture);
            renderer.glContext.TextureParameter(texture, TextureParameterName.TextureWrapS,
                (int)GetTextureWrap(this.wrapModeS));
            renderer.glContext.TextureParameter(texture, TextureParameterName.TextureWrapT,
                (int)GetTextureWrap(this.wrapModeT));
            renderer.glContext.TextureParameter(texture, TextureParameterName.TextureWrapR,
                (int)GetTextureWrap(this.wrapModeR));

            renderer.glContext.BindTexture(GetTextureTarget(targetMode), 0); //Unbind the texture.
        }

        public override void UpdateFilterMode(MinFilterMode? minFilterMode = null, MagFilterMode? magFilterMode = null)
        {
            this.minFilterMode = minFilterMode ?? this.minFilterMode;
            this.magFilterMode = magFilterMode ?? this.magFilterMode;

            renderer.glContext.BindTexture(GetTextureTarget(targetMode), texture);
            renderer.glContext.TextureParameter(texture, TextureParameterName.TextureMinFilter,
                (int)GetTextureMinFilter(this.minFilterMode));
            renderer.glContext.TextureParameter(texture, TextureParameterName.TextureMagFilter,
                (int)GetTextureMagFilter(this.magFilterMode));

            renderer.glContext.BindTexture(GetTextureTarget(targetMode), 0); //Unbind the texture.
        }

        public override void Assign(int unit = 0)
        {
            renderer.glContext.ActiveTexture(TextureUnit.Texture0 + unit);
        }

        public override void Render(double delta)
        {
            renderer.glContext.BindTexture(GetTextureTarget(targetMode), texture);
        }
        
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if(disposing)
            {
                renderer.glContext.DeleteTexture(texture);
            }

            IsDisposed = true;
        }

        ~Texture()
        {
            Dispose(false);
        }

        private static TextureWrapMode GetTextureWrap(WrapMode wrapMode)
        {
            return wrapMode switch
            {
                WrapMode.Repeat => TextureWrapMode.Repeat,
                _ => throw new ArgumentOutOfRangeException(nameof(wrapMode))
            };
        }

        private static TextureMinFilter GetTextureMinFilter(MinFilterMode minFilterMode)
        {
            return minFilterMode switch
            {
                MinFilterMode.Linear => TextureMinFilter.Linear,
                MinFilterMode.Nearest => TextureMinFilter.Nearest,
                _ => throw new ArgumentOutOfRangeException(nameof(minFilterMode))
            };
        }

        private static TextureMagFilter GetTextureMagFilter(MagFilterMode magFilterMode)
        {
            return magFilterMode switch
            {
                MagFilterMode.Linear => TextureMagFilter.Linear,
                MagFilterMode.Nearest => TextureMagFilter.Nearest,
                _ => throw new ArgumentOutOfRangeException(nameof(magFilterMode))
            };
        }

        private static InternalFormat GetInternalFormat(TextureFormatMode formatMode)
        {
            return formatMode switch
            {
                TextureFormatMode.RGB => InternalFormat.Rgb,
                TextureFormatMode.RGBA => InternalFormat.Rgba,
                _ => throw new ArgumentOutOfRangeException(nameof(formatMode))
            };
        }

        private static PixelFormat GetPixelFormat(TextureFormatMode formatMode)
        {
            return formatMode switch
            {
                TextureFormatMode.RGB => PixelFormat.Rgb,
                TextureFormatMode.RGBA => PixelFormat.Rgba,
                _ => throw new ArgumentOutOfRangeException(nameof(formatMode))
            };
        }

        private static TextureTarget GetTextureTarget(TextureTargetMode targetMode)
        {
            return targetMode switch
            {
                TextureTargetMode.Texture1D => TextureTarget.Texture1D,
                TextureTargetMode.Texture2D => TextureTarget.Texture2D,
                TextureTargetMode.Texture3D => TextureTarget.Texture3D,
                TextureTargetMode.CubeMap => TextureTarget.TextureCubeMap,
                _ => throw new ArgumentOutOfRangeException(nameof(targetMode))
            };
        }
    }
}