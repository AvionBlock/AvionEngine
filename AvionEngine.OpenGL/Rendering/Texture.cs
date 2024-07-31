using AvionEngine.Enums;
using AvionEngine.Interfaces;
using Silk.NET.OpenGL;
using System;

namespace AvionEngine.OpenGL.Rendering
{
    public class Texture : ITexture
    {
        private GL glInstance;
        private uint texture;

        public unsafe Texture(GL glInstance)
        {
            this.glInstance = glInstance;

            fixed (uint* texturePtr = &texture)
                glInstance.GenTextures(1, texturePtr);
        }

        public unsafe void Set(uint width, uint height, byte[] data)
        {
            glInstance.BindTexture(TextureTarget.Texture2D, texture);

            fixed (byte* dataPtr = data)
                glInstance.TexImage2D(GLEnum.Texture2D, 0, InternalFormat.Rgba, width, height, 0, GLEnum.Rgba, GLEnum.UnsignedByte, dataPtr);

            glInstance.GenerateMipmap(TextureTarget.Texture2D);

            glInstance.BindTexture(TextureTarget.Texture2D, 0); //Unbind the texture.
        }

        public void SetWrap(WrapMode mode)
        {
            glInstance.BindTexture(TextureTarget.Texture2D, texture);
            glInstance.TextureParameter(texture, TextureParameterName.TextureWrapS, (int)GetTextureWrapMode(mode));
            glInstance.TextureParameter(texture, TextureParameterName.TextureWrapS, (int)GetTextureWrapMode(mode));

            glInstance.BindTexture(TextureTarget.Texture2D, 0); //Unbind the texture.
        }

        public void SetFilter(MinFilterMode min, MagFilterMode mag)
        {
            glInstance.BindTexture(TextureTarget.Texture2D, texture);
            glInstance.TextureParameter(texture, TextureParameterName.TextureMinFilter, (int)GetTextureMinFilter(min));
            glInstance.TextureParameter(texture, TextureParameterName.TextureMagFilter, (int)GetTextureMagFilter(mag));

            glInstance.BindTexture(TextureTarget.Texture2D, 0); //Unbind the texture.
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
                default:
                    return TextureMinFilter.Linear;
            }
        }

        private static TextureMagFilter GetTextureMagFilter(MagFilterMode mode)
        {
            switch (mode)
            {
                default:
                    return TextureMagFilter.Linear;
            }
        }
    }
}
