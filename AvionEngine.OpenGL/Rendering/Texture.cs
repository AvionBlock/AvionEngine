using AvionEngine.Enums;
using AvionEngine.Interfaces;
using Silk.NET.OpenGL;
using System;

namespace AvionEngine.OpenGL.Rendering
{
    public class Texture : ITexture
    {
        private static int allocatedUnits;
        private GL glInstance;
        private uint texture;
        private Enums.TextureTarget target;

        public unsafe Texture(GL glInstance, uint width, uint height, byte[] data, TextureFormat format = TextureFormat.RGB, Enums.TextureTarget target = Enums.TextureTarget.Texture2D)
        {
            this.glInstance = glInstance;
            this.target = target;

            fixed (uint* texturePtr = &texture)
                glInstance.GenTextures(1, texturePtr);

            Set(width, height, data, format);
        }

        public void Assign()
        {
            var max = glInstance.GetInteger(GetPName.MaxCombinedTextureImageUnits);
            if (allocatedUnits >= max)
            {
                throw new Exception($"Error, Cannot have more than {max} active textures per draw call!");
            }

            glInstance.ActiveTexture(TextureUnit.Texture0 + allocatedUnits);
            allocatedUnits++;
        }

        public void Render()
        {
            glInstance.BindTexture(GetTextureTarget(target), texture);
            allocatedUnits = 0;
        }

        public unsafe void Set(uint width, uint height, byte[] data, TextureFormat format = TextureFormat.RGB)
        {
            glInstance.BindTexture(GetTextureTarget(target), texture);

            fixed (byte* dataPtr = data)
                glInstance.TexImage2D(GetTextureTarget(target), 0, GetTextureFormat(format), width, height, 0, GetTextureFormatGL(format), GLEnum.UnsignedByte, dataPtr);

            glInstance.GenerateMipmap(GetTextureTarget(target));

            glInstance.BindTexture(GetTextureTarget(target), 0); //Unbind the texture.
        }

        public void SetWrap(WrapMode wrapS, WrapMode wrapT)
        {
            glInstance.BindTexture(GetTextureTarget(target), texture);
            glInstance.TextureParameter(texture, TextureParameterName.TextureWrapS, (int)GetTextureWrapMode(wrapS));
            glInstance.TextureParameter(texture, TextureParameterName.TextureWrapT, (int)GetTextureWrapMode(wrapT));

            glInstance.BindTexture(GetTextureTarget(target), 0); //Unbind the texture.
        }

        public void SetFilter(MinFilterMode min, MagFilterMode mag)
        {
            glInstance.BindTexture(GetTextureTarget(target), texture);
            glInstance.TextureParameter(texture, TextureParameterName.TextureMinFilter, (int)GetTextureMinFilter(min));
            glInstance.TextureParameter(texture, TextureParameterName.TextureMagFilter, (int)GetTextureMagFilter(mag));

            glInstance.BindTexture(GetTextureTarget(target), 0); //Unbind the texture.
        }

        private static TextureWrapMode GetTextureWrapMode(WrapMode mode)
        {
            switch (mode)
            {
                default:
                    return TextureWrapMode.Repeat;
            }
        }

        private static TextureMinFilter GetTextureMinFilter(MinFilterMode mode)
        {
            switch (mode)
            {
                case MinFilterMode.Nearest:
                    return TextureMinFilter.Nearest;
                default:
                    return TextureMinFilter.Linear;
            }
        }

        private static TextureMagFilter GetTextureMagFilter(MagFilterMode mode)
        {
            switch (mode)
            {
                case MagFilterMode.Nearest:
                    return TextureMagFilter.Nearest;
                default:
                    return TextureMagFilter.Linear;
            }
        }

        private static InternalFormat GetTextureFormat(TextureFormat format)
        {
            switch (format)
            {
                case TextureFormat.RGBA:
                    return InternalFormat.Rgba;
                default:
                    return InternalFormat.Rgb;
            }
        }

        private static GLEnum GetTextureFormatGL(TextureFormat format)
        {
            switch (format)
            {
                case TextureFormat.RGBA:
                    return GLEnum.Rgba;
                default:
                    return GLEnum.Rgb;
            }
        }

        private static Silk.NET.OpenGL.TextureTarget GetTextureTarget(Enums.TextureTarget target)
        {
            switch (target)
            {
                case Enums.TextureTarget.Texture1D:
                    return Silk.NET.OpenGL.TextureTarget.Texture1D;
                case Enums.TextureTarget.Texture3D:
                    return Silk.NET.OpenGL.TextureTarget.Texture3D;
                case Enums.TextureTarget.CubeMap:
                    return Silk.NET.OpenGL.TextureTarget.TextureCubeMap;
                default:
                    return Silk.NET.OpenGL.TextureTarget.Texture2D;
            }
        }
    }
}
