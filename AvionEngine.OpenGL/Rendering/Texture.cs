using System;
using AvionEngine.Enums;
using AvionEngine.Interfaces;
using Silk.NET.OpenGL;

namespace AvionEngine.OpenGL.Rendering
{
    public class Texture : ITexture
    {
        private readonly GL glInstance;
        private readonly uint texture;
        private TextureTargetMode targetMode;
        private TextureFormatMode formatMode;
        private WrapMode wrapModeS;
        private WrapMode wrapModeT;
        private WrapMode wrapModeR;
        private MinFilterMode minFilterMode;
        private MagFilterMode magFilterMode;

        public Texture(GL glInstance,
            uint width,
            uint height,
            byte[] data,
            uint depth = 0,
            TextureTargetMode targetMode = TextureTargetMode.Texture2D,
            TextureFormatMode formatMode = TextureFormatMode.RGB,
            WrapMode wrapModeS = WrapMode.Repeat,
            WrapMode wrapModeT = WrapMode.Repeat,
            WrapMode wrapModeR = WrapMode.Repeat,
            MinFilterMode minFilterMode = MinFilterMode.Linear,
            MagFilterMode magFilterMode = MagFilterMode.Linear)
        {
            this.glInstance = glInstance;
            this.targetMode = targetMode;
            this.formatMode = formatMode;
            this.wrapModeS = wrapModeS;
            this.wrapModeT = wrapModeT;
            this.wrapModeR = wrapModeR;
            this.minFilterMode = minFilterMode;
            this.magFilterMode = magFilterMode;

            texture = glInstance.GenTexture();
            Update(width, height, data, depth);
        }

        public unsafe void Update(uint width, uint height, byte[] data, uint depth = 0,
            TextureTargetMode? targetMode = null, TextureFormatMode? formatMode = null)
        {
            this.targetMode = targetMode ?? this.targetMode;
            this.formatMode = formatMode ?? this.formatMode;

            glInstance.BindTexture(GetTextureTarget(this.targetMode), texture);
            fixed (byte* dataPtr = data)
            {
                switch (this.targetMode)
                {
                    case TextureTargetMode.Texture1D:
                        glInstance.TexImage1D(GetTextureTarget(this.targetMode), 0, GetInternalFormat(this.formatMode),
                            width, 0, GetPixelFormat(this.formatMode), GLEnum.UnsignedByte, dataPtr);
                        break;
                    case TextureTargetMode.Texture2D:
                        glInstance.TexImage2D(GetTextureTarget(this.targetMode), 0, GetInternalFormat(this.formatMode),
                            width, height, 0, GetPixelFormat(this.formatMode), GLEnum.UnsignedByte, dataPtr);
                        break;
                    case TextureTargetMode.Texture3D:
                        glInstance.TexImage3D(GetTextureTarget(this.targetMode), 0, GetInternalFormat(this.formatMode),
                            width, height, depth, 0, GetPixelFormat(this.formatMode), GLEnum.UnsignedByte, dataPtr);
                        break;
                    case TextureTargetMode.CubeMap:
                    default:
                        throw new InvalidOperationException(
                            "Cannot apply this texture update with the specified target mode.");
                }
            }

            glInstance.GenerateMipmap(GetTextureTarget(this.targetMode));
            glInstance.BindTexture(GetTextureTarget(this.targetMode), 0); //Unbind Texture.
        }

        public unsafe void Update(uint[] width, uint[] height, uint[] depth, byte[][] data,
            TextureTargetMode? targetMode = null, TextureFormatMode? formatMode = null)
        {
            this.targetMode = targetMode ?? this.targetMode;
            this.formatMode = formatMode ?? this.formatMode;

            glInstance.BindTexture(GetTextureTarget(this.targetMode), texture);
            switch (this.targetMode)
            {
                case TextureTargetMode.CubeMap:
                    for (var i = 0; i < data.Length; i++)
                    {
                        fixed (byte* dataPtr = data[i])
                        {
                            glInstance.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0,
                                GetInternalFormat(this.formatMode), width[i], height[i], 0,
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

            glInstance.GenerateMipmap(GetTextureTarget(this.targetMode));
            glInstance.BindTexture(GetTextureTarget(this.targetMode), 0); //Unbind Texture.
        }

        public void UpdateWrapMode(WrapMode? wrapModeS = null, WrapMode? wrapModeT = null, WrapMode? wrapModeR = null)
        {
            this.wrapModeS = wrapModeS ?? this.wrapModeS;
            this.wrapModeT = wrapModeT ?? this.wrapModeT;
            this.wrapModeR = wrapModeR ?? this.wrapModeR;

            glInstance.BindTexture(GetTextureTarget(targetMode), texture);
            glInstance.TextureParameter(texture, TextureParameterName.TextureWrapS,
                (int)GetTextureWrap(this.wrapModeS));
            glInstance.TextureParameter(texture, TextureParameterName.TextureWrapT,
                (int)GetTextureWrap(this.wrapModeT));
            glInstance.TextureParameter(texture, TextureParameterName.TextureWrapR,
                (int)GetTextureWrap(this.wrapModeR));

            glInstance.BindTexture(GetTextureTarget(targetMode), 0); //Unbind the texture.
        }

        public void UpdateFilterMode(MinFilterMode? minFilterMode = null, MagFilterMode? magFilterMode = null)
        {
            this.minFilterMode = minFilterMode ?? this.minFilterMode;
            this.magFilterMode = magFilterMode ?? this.magFilterMode;

            glInstance.BindTexture(GetTextureTarget(targetMode), texture);
            glInstance.TextureParameter(texture, TextureParameterName.TextureMinFilter,
                (int)GetTextureMinFilter(this.minFilterMode));
            glInstance.TextureParameter(texture, TextureParameterName.TextureMagFilter,
                (int)GetTextureMagFilter(this.magFilterMode));

            glInstance.BindTexture(GetTextureTarget(targetMode), 0); //Unbind the texture.
        }

        public void Render(int unit = 0)
        {
            glInstance.ActiveTexture(TextureUnit.Texture0 + unit);
            glInstance.BindTexture(GetTextureTarget(targetMode), texture);
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