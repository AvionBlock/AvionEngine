using AvionEngine.Enums;
using AvionEngine.Interfaces;
using Silk.NET.OpenGL;

namespace AvionEngine.OpenGL.Rendering
{
    public class Texture : ITexture
    {
        private GL glInstance;
        private uint texture;
        private TextureTargetMode targetMode;
        private TextureFormatMode formatMode;
        private WrapMode wrapModeS;
        private WrapMode wrapModeT;
        private WrapMode wrapModeR;
        private MinFilterMode minFilterMode;
        private MagFilterMode magFilterMode;

        public unsafe Texture(GL glInstance,
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

        public unsafe void Update(uint width, uint height, byte[] data, uint depth = 0)
        {
            glInstance.BindTexture(GetTextureTarget(targetMode), texture);
            fixed (byte* dataPtr = data)
            {
                switch (this.targetMode) {
                    case TextureTargetMode.Texture1D:
                        glInstance.TexImage1D(GetTextureTarget(this.targetMode), 0, GetInternalFormat(this.formatMode), width, 0, GetPixelFormat(this.formatMode), GLEnum.UnsignedByte, dataPtr);
                        break;
                    case TextureTargetMode.Texture2D:
                        glInstance.TexImage2D(GetTextureTarget(this.targetMode), 0, GetInternalFormat(this.formatMode), width, height, 0, GetPixelFormat(this.formatMode), GLEnum.UnsignedByte, dataPtr);
                        break;
                    case TextureTargetMode.Texture3D:
                        glInstance.TexImage3D(GetTextureTarget(this.targetMode), 0, GetInternalFormat(this.formatMode), width, height, depth, 0, GetPixelFormat(this.formatMode), GLEnum.UnsignedByte, dataPtr);
                        break;
                        /*Leave for now.
                    case TextureTargetMode.CubeMap:
                        glInstance.TexImage2D(TextureTarget.TextureCubeMapPositiveX, 0, GetInternalFormat(this.formatMode), width, height, 0, GetPixelFormat(this.formatMode), GLEnum.UnsignedByte, dataPtr);
                        break;
                        */
                }
            }
            glInstance.GenerateMipmap(GetTextureTarget(targetMode));
            glInstance.BindTexture(GetTextureTarget(targetMode), 0); //Unbind Texture.
        }

        public void UpdateWrapMode(WrapMode? wrapModeS = null, WrapMode? wrapModeT = null, WrapMode? wrapModeR = null)
        {
            this.wrapModeS = wrapModeS ?? this.wrapModeS;
            this.wrapModeT = wrapModeT ?? this.wrapModeT;

            glInstance.BindTexture(GetTextureTarget(targetMode), texture);
            glInstance.TextureParameter(texture, TextureParameterName.TextureWrapS, (int)GetTextureWrap(this.wrapModeS));
            glInstance.TextureParameter(texture, TextureParameterName.TextureWrapT, (int)GetTextureWrap(this.wrapModeT));
            glInstance.TextureParameter(texture, TextureParameterName.TextureWrapR, (int)GetTextureWrap(this.wrapModeR));

            glInstance.BindTexture(GetTextureTarget(targetMode), 0); //Unbind the texture.
        }

        public void UpdateFilterMode(MinFilterMode? minFilterMode = null, MagFilterMode? magFilterMode = null)
        {
            this.minFilterMode = minFilterMode ?? this.minFilterMode;
            this.magFilterMode = magFilterMode ?? this.magFilterMode;

            glInstance.BindTexture(GetTextureTarget(targetMode), texture);
            glInstance.TextureParameter(texture, TextureParameterName.TextureMinFilter, (int)GetTextureMinFilter(this.minFilterMode));
            glInstance.TextureParameter(texture, TextureParameterName.TextureMagFilter, (int)GetTextureMagFilter(this.magFilterMode));

            glInstance.BindTexture(GetTextureTarget(targetMode), 0); //Unbind the texture.
        }

        public void Render(int unit = 0)
        {
            glInstance.ActiveTexture(TextureUnit.Texture0 + unit);
            glInstance.BindTexture(GetTextureTarget(targetMode), texture);
        }

        private static TextureWrapMode GetTextureWrap(WrapMode mode)
        {
            switch (mode)
            {
                default:
                    return TextureWrapMode.Repeat;
            }
        }

        private static TextureMinFilter GetTextureMinFilter(MinFilterMode minFilterMode)
        {
            switch (minFilterMode)
            {
                case MinFilterMode.Nearest:
                    return TextureMinFilter.Nearest;
                default:
                    return TextureMinFilter.Linear;
            }
        }

        private static TextureMagFilter GetTextureMagFilter(MagFilterMode magFilterMode)
        {
            switch (magFilterMode)
            {
                case MagFilterMode.Nearest:
                    return TextureMagFilter.Nearest;
                default:
                    return TextureMagFilter.Linear;
            }
        }

        private static InternalFormat GetInternalFormat(TextureFormatMode formatMode)
        {
            switch (formatMode)
            {
                case TextureFormatMode.RGBA:
                    return InternalFormat.Rgba;
                default:
                    return InternalFormat.Rgb;
            }
        }

        private static PixelFormat GetPixelFormat(TextureFormatMode formatMode)
        {
            switch (formatMode)
            {
                case TextureFormatMode.RGBA:
                    return PixelFormat.Rgba;
                default:
                    return PixelFormat.Rgb;
            }
        }

        private static TextureTarget GetTextureTarget(TextureTargetMode targetMode)
        {
            switch (targetMode)
            {
                case TextureTargetMode.Texture1D:
                    return TextureTarget.Texture1D;
                case TextureTargetMode.Texture3D:
                    return TextureTarget.Texture3D;
                case TextureTargetMode.CubeMap:
                    return TextureTarget.TextureCubeMap;
                default:
                    return TextureTarget.Texture2D;
            }
        }
    }
}
