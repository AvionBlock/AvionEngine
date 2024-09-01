using AvionEngine.Descriptors;
using AvionEngine.Graphics;
using Silk.NET.OpenGL;
using System;

namespace AvionEngine.OpenGL.Graphics
{
    public class GLTexture : AVTexture
    {
        public readonly GLRenderer renderer;

        public readonly uint Texture;
        public readonly SizedInternalFormat InternalFormat;
        public readonly PixelFormat PixelFormat;
        public readonly PixelType PixelType;

        public GLTexture(GLRenderer renderer, TextureType textureType, TextureDescriptor textureDescriptor) : base(textureType, textureDescriptor.Format)
        {
            this.renderer = renderer;

            (InternalFormat, PixelFormat, PixelType) = GetTextureFormat(textureDescriptor.Format);

            Texture = renderer.glContext.GenTexture();
            Update(textureDescriptor);
        }

        public override unsafe void Update(TextureDescriptor textureDescriptor)
        {
            var isCompressed = FormatIsCompressed(textureDescriptor.Format);
            renderer.glContext.BindTexture(GetTextureTarget(TextureType), Texture);
            switch (TextureType)
            {
                case TextureType.Texture1D:
                    renderer.glContext.TexStorage1D(GetTextureTarget(TextureType), textureDescriptor.MipLevels, InternalFormat, textureDescriptor.Width);

                    fixed (void* dataPtr = textureDescriptor.Data[0])
                    {
                        if (isCompressed)
                        {
                            renderer.glContext.CompressedTexSubImage1D(GetTextureTarget(TextureType), 0, 0, textureDescriptor.Width, (InternalFormat)InternalFormat, CalculateTextureSizeInBytes(textureDescriptor.Format, textureDescriptor.Width, textureDescriptor.Height), dataPtr);
                        }
                        else
                        {
                            renderer.glContext.TexSubImage1D(GetTextureTarget(TextureType), 0, 0, textureDescriptor.Width, PixelFormat, PixelType, dataPtr);
                        }
                    }

                    if (textureDescriptor.MipLevels == 0)
                        renderer.glContext.GenerateMipmap(GetTextureTarget(TextureType));
                    break;
                case TextureType.Texture2D:
                    renderer.glContext.TexStorage2D(GetTextureTarget(TextureType), textureDescriptor.MipLevels, InternalFormat, textureDescriptor.Width, textureDescriptor.Height);

                    fixed (void* dataPtr = textureDescriptor.Data[0])
                    {
                        if (isCompressed)
                        {
                            renderer.glContext.CompressedTexSubImage2D(GetTextureTarget(TextureType), 0, 0, 0, textureDescriptor.Width, textureDescriptor.Height, (InternalFormat)InternalFormat, CalculateTextureSizeInBytes(textureDescriptor.Format, textureDescriptor.Width, textureDescriptor.Height), dataPtr);
                        }
                        else
                        {
                            renderer.glContext.TexSubImage2D(GetTextureTarget(TextureType), 0, 0, 0, textureDescriptor.Width, textureDescriptor.Height, PixelFormat, PixelType, dataPtr);
                        }
                    }

                    if (textureDescriptor.MipLevels == 0)
                        renderer.glContext.GenerateMipmap(GetTextureTarget(TextureType));
                    break;
                case TextureType.Texture3D:
                    renderer.glContext.TexStorage3D(GetTextureTarget(TextureType), textureDescriptor.MipLevels, InternalFormat, textureDescriptor.Width, textureDescriptor.Height, textureDescriptor.Depth);

                    fixed (void* dataPtr = textureDescriptor.Data[0])
                    {
                        if (isCompressed)
                        {
                            renderer.glContext.CompressedTexSubImage3D(GetTextureTarget(TextureType), 0, 0, 0, 0, textureDescriptor.Width, textureDescriptor.Height, textureDescriptor.Depth, (InternalFormat)InternalFormat, CalculateTextureSizeInBytes(textureDescriptor.Format, textureDescriptor.Width, textureDescriptor.Height), dataPtr);
                        }
                        else
                        {
                            renderer.glContext.TexSubImage3D(GetTextureTarget(TextureType), 0, 0, 0, 0, textureDescriptor.Width, textureDescriptor.Height, textureDescriptor.Depth, PixelFormat, PixelType, dataPtr);
                        }
                    }

                    if (textureDescriptor.MipLevels == 0)
                        renderer.glContext.GenerateMipmap(GetTextureTarget(TextureType));
                    break;
                case TextureType.CubeMap:
                    renderer.glContext.TexStorage2D(GetTextureTarget(TextureType), textureDescriptor.MipLevels, InternalFormat, textureDescriptor.Width, textureDescriptor.Height);

                    for (int a = 0; a < 6; a++)
                    {
                        TextureTarget target = TextureTarget.TextureCubeMapPositiveX + a;
                        fixed (void* dataPtr = textureDescriptor.Data[a])
                        {
                            if (isCompressed)
                            {
                                renderer.glContext.CompressedTexSubImage2D(target, 0, 0, 0, textureDescriptor.Width, textureDescriptor.Height, (InternalFormat)InternalFormat, CalculateTextureSizeInBytes(textureDescriptor.Format, textureDescriptor.Width, textureDescriptor.Height), dataPtr);
                            }
                            else
                            {
                                renderer.glContext.TexSubImage2D(GetTextureTarget(TextureType), 0, 0, 0, textureDescriptor.Width, textureDescriptor.Height, PixelFormat, PixelType, dataPtr);
                            }
                        }

                        if (textureDescriptor.MipLevels == 0)
                            renderer.glContext.GenerateMipmap(GetTextureTarget(TextureType));
                    }
                    break;

                default:
                    throw new NotSupportedException($"{textureDescriptor.Format} is not supported!");
            }
        }

        public override void SetWrapMode(TextureWrapMode? textureWrapModeS = null, TextureWrapMode? textureWrapModeT = null, TextureWrapMode? textureWrapModeR = null)
        {
            base.SetWrapMode(textureWrapModeS, textureWrapModeT, textureWrapModeR);

            renderer.glContext.BindTexture(GetTextureTarget(TextureType), Texture);
            renderer.glContext.TextureParameter(Texture, TextureParameterName.TextureWrapS, (int)GetTextureWrap(TextureWrapModeS));
            renderer.glContext.TextureParameter(Texture, TextureParameterName.TextureWrapT, (int)GetTextureWrap(TextureWrapModeT));
            renderer.glContext.TextureParameter(Texture, TextureParameterName.TextureWrapR, (int)GetTextureWrap(TextureWrapModeR));

            renderer.glContext.BindTexture(GetTextureTarget(TextureType), 0); //Unbind the texture.
        }

        public override void SetFilterMode(MinFilterMode? minFilterMode = null, MagFilterMode? magFilterMode = null)
        {
            base.SetFilterMode(minFilterMode, magFilterMode);

            renderer.glContext.BindTexture(GetTextureTarget(TextureType), Texture);
            renderer.glContext.TextureParameter(Texture, TextureParameterName.TextureMinFilter,
                (int)GetTextureMinFilter(MinFilterMode));
            renderer.glContext.TextureParameter(Texture, TextureParameterName.TextureMagFilter,
                (int)GetTextureMagFilter(MagFilterMode));

            renderer.glContext.BindTexture(GetTextureTarget(TextureType), 0); //Unbind the texture.
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
                renderer.glContext.DeleteTexture(Texture);
            }

            IsDisposed = true;
        }

        ~GLTexture()
        {
            Dispose(false);
        }

        //https://github.com/Aquatic-Games/grabs/blob/main/src/grabs.Graphics.GL43/GL43Texture.cs
        public static (SizedInternalFormat, PixelFormat, PixelType) GetTextureFormat(FormatType formatType)
        {
            return formatType switch
            {
                FormatType.R8G8B8A8_UNorm => (SizedInternalFormat.Rgba8, PixelFormat.Rgba, PixelType.UnsignedByte),
                FormatType.B5G6R5_UNorm => (SizedInternalFormat.Rgb565, PixelFormat.Bgr, PixelType.UnsignedShort565),
                FormatType.B5G5R5A1_UNorm => (SizedInternalFormat.Rgba8, PixelFormat.Bgra, PixelType.UnsignedShort5551),
                FormatType.R8_UNorm => (SizedInternalFormat.R8, PixelFormat.Red, PixelType.UnsignedByte),
                FormatType.R8_UInt => (SizedInternalFormat.R8i, PixelFormat.Red, PixelType.UnsignedInt),
                FormatType.R8_SNorm => (SizedInternalFormat.R8SNorm, PixelFormat.Red, PixelType.Byte),
                FormatType.R8_SInt => (SizedInternalFormat.R8i, PixelFormat.Red, PixelType.Int),
                FormatType.A8_UNorm => (SizedInternalFormat.Alpha8Ext, PixelFormat.Alpha, PixelType.UnsignedByte),
                FormatType.R8G8_UNorm => (SizedInternalFormat.RG8, PixelFormat.RG, PixelType.UnsignedByte),
                FormatType.R8G8_UInt => (SizedInternalFormat.RG8, PixelFormat.RG, PixelType.UnsignedInt),
                FormatType.R8G8_SNorm => (SizedInternalFormat.RG8, PixelFormat.RG, PixelType.Byte),
                FormatType.R8G8_SInt => (SizedInternalFormat.RG8, PixelFormat.RG, PixelType.Int),
                FormatType.R8G8B8A8_UNorm_SRGB => (SizedInternalFormat.Srgb8Alpha8, PixelFormat.Rgba, PixelType.UnsignedByte),
                FormatType.R8G8B8A8_UInt => (SizedInternalFormat.Rgba8i, PixelFormat.Rgba, PixelType.UnsignedInt),
                FormatType.R8G8B8A8_SNorm => (SizedInternalFormat.Rgba8, PixelFormat.Rgba, PixelType.Byte),
                FormatType.R8G8B8A8_SInt => (SizedInternalFormat.Rgba8i, PixelFormat.Rgba, PixelType.Int),
                FormatType.B8G8R8A8_UNorm => (SizedInternalFormat.Rgba8, PixelFormat.Bgra, PixelType.UnsignedByte),
                FormatType.B8G8R8A8_UNorm_SRGB => (SizedInternalFormat.Srgb8Alpha8, PixelFormat.Bgra, PixelType.UnsignedByte),
                FormatType.R10G10B10A2_UNorm => (SizedInternalFormat.Rgb10A2, PixelFormat.Rgba, PixelType.UnsignedInt1010102),
                FormatType.R10G10B10A2_UInt => (SizedInternalFormat.Rgb10A2ui, PixelFormat.Rgba, PixelType.UnsignedInt1010102),
                FormatType.R11G11B10_Float => (SizedInternalFormat.R11fG11fB10f, PixelFormat.Rgb, PixelType.Float),
                FormatType.R16_Float => (SizedInternalFormat.R16f, PixelFormat.Red, PixelType.Float),
                FormatType.D16_UNorm => (SizedInternalFormat.DepthComponent16, PixelFormat.DepthComponent, PixelType.UnsignedShort),
                FormatType.R16_UNorm => (SizedInternalFormat.R16, PixelFormat.Red, PixelType.UnsignedShort),
                FormatType.R16_UInt => (SizedInternalFormat.R16ui, PixelFormat.Red, PixelType.UnsignedInt),
                FormatType.R16_SNorm => (SizedInternalFormat.R16SNorm, PixelFormat.Red, PixelType.Short),
                FormatType.R16_SInt => (SizedInternalFormat.R16i, PixelFormat.Red, PixelType.Int),
                FormatType.R16G16_Float => (SizedInternalFormat.RG16f, PixelFormat.RG, PixelType.Float),
                FormatType.R16G16_UNorm => (SizedInternalFormat.RG16, PixelFormat.RG, PixelType.UnsignedShort),
                FormatType.R16G16_UInt => (SizedInternalFormat.RG16ui, PixelFormat.RG, PixelType.UnsignedInt),
                FormatType.R16G16_SNorm => (SizedInternalFormat.RG16SNorm, PixelFormat.RG, PixelType.Short),
                FormatType.R16G16_SInt => (SizedInternalFormat.RG16i, PixelFormat.RG, PixelType.Int),
                FormatType.R16G16B16A16_Float => (SizedInternalFormat.Rgb16f, PixelFormat.Rgba, PixelType.Float),
                FormatType.R16G16B16A16_UNorm => (SizedInternalFormat.Rgba16, PixelFormat.Rgba, PixelType.UnsignedShort),
                FormatType.R16G16B16A16_UInt => (SizedInternalFormat.Rgba16ui, PixelFormat.Rgba, PixelType.UnsignedInt),
                FormatType.R16G16B16A16_SNorm => (SizedInternalFormat.Rgba16SNorm, PixelFormat.Rgba, PixelType.Short),
                FormatType.R16G16B16A16_SInt => (SizedInternalFormat.Rgba16i, PixelFormat.Rgba, PixelType.Int),
                FormatType.R32_Float => (SizedInternalFormat.R32f, PixelFormat.Red, PixelType.Float),
                FormatType.R32_UInt => (SizedInternalFormat.R32ui, PixelFormat.Red, PixelType.UnsignedInt),
                FormatType.R32_SInt => (SizedInternalFormat.R32i, PixelFormat.Red, PixelType.Int),
                FormatType.R32G32_Float => (SizedInternalFormat.RG32f, PixelFormat.RG, PixelType.Float),
                FormatType.R32G32_UInt => (SizedInternalFormat.RG32ui, PixelFormat.RG, PixelType.UnsignedInt),
                FormatType.R32G32_SInt => (SizedInternalFormat.RG32i, PixelFormat.RG, PixelType.Int),
                FormatType.R32G32B32_Float => (SizedInternalFormat.Rgb32f, PixelFormat.Rgb, PixelType.Float),
                FormatType.R32G32B32_UInt => (SizedInternalFormat.Rgb32ui, PixelFormat.Rgb, PixelType.UnsignedInt),
                FormatType.R32G32B32_SInt => (SizedInternalFormat.Rgb32i, PixelFormat.Rgb, PixelType.Int),
                FormatType.R32G32B32A32_Float => (SizedInternalFormat.Rgba32f, PixelFormat.Rgba, PixelType.Float),
                FormatType.R32G32B32A32_UInt => (SizedInternalFormat.Rgba32ui, PixelFormat.Rgba, PixelType.UnsignedInt),
                FormatType.R32G32B32A32_SInt => (SizedInternalFormat.Rgba32i, PixelFormat.Rgba, PixelType.Int),
                FormatType.D24_UNorm_S8_UInt => (SizedInternalFormat.Depth24Stencil8, PixelFormat.DepthStencil, PixelType.UnsignedByte), // ????
                FormatType.D32_Float => (SizedInternalFormat.DepthComponent32f, PixelFormat.DepthComponent, PixelType.Float),
                FormatType.BC1_UNorm => (SizedInternalFormat.CompressedRgbaS3TCDxt1Ext, PixelFormat.Rgba, PixelType.UnsignedByte),
                FormatType.BC1_UNorm_SRGB => (SizedInternalFormat.CompressedSrgbAlphaS3TCDxt1Ext, PixelFormat.Rgba, PixelType.UnsignedByte),
                FormatType.BC2_UNorm => (SizedInternalFormat.CompressedRgbaS3TCDxt3Ext, PixelFormat.Rgba, PixelType.UnsignedByte),
                FormatType.BC2_UNorm_SRGB => (SizedInternalFormat.CompressedSrgbAlphaS3TCDxt3Ext, PixelFormat.Rgba, PixelType.UnsignedByte),
                FormatType.BC3_UNorm => (SizedInternalFormat.CompressedRgbaS3TCDxt5Ext, PixelFormat.Rgba, PixelType.UnsignedByte),
                FormatType.BC3_UNorm_SRGB => (SizedInternalFormat.CompressedSrgbAlphaS3TCDxt5Ext, PixelFormat.Rgba, PixelType.UnsignedByte),
                FormatType.BC4_UNorm => (SizedInternalFormat.CompressedRedRgtc1, PixelFormat.Red, PixelType.UnsignedByte),
                FormatType.BC4_SNorm => (SizedInternalFormat.CompressedSignedRedRgtc1, PixelFormat.Red, PixelType.UnsignedByte),
                FormatType.BC5_UNorm => (SizedInternalFormat.CompressedRGRgtc2, PixelFormat.RG, PixelType.UnsignedByte),
                FormatType.BC5_SNorm => (SizedInternalFormat.CompressedSignedRGRgtc2, PixelFormat.RG, PixelType.UnsignedByte),
                FormatType.BC6H_UF16 => (SizedInternalFormat.CompressedRgbBptcUnsignedFloat, PixelFormat.Rgb, PixelType.UnsignedByte),
                FormatType.BC6H_SF16 => (SizedInternalFormat.CompressedRgbBptcSignedFloat, PixelFormat.Rgb, PixelType.UnsignedByte),
                FormatType.BC7_UNorm => (SizedInternalFormat.CompressedRgbaBptcUnorm, PixelFormat.Rgba, PixelType.UnsignedByte),
                FormatType.BC7_UNorm_SRGB => (SizedInternalFormat.CompressedSrgbAlphaBptcUnorm, PixelFormat.Rgba, PixelType.UnsignedByte),
                _ => throw new NotImplementedException()
            };
        }

        public static TextureTarget GetTextureTarget(TextureType textureType)
        {
            return textureType switch
            {
                TextureType.Texture1D => TextureTarget.Texture1D,
                TextureType.Texture2D => TextureTarget.Texture2D,
                TextureType.Texture3D => TextureTarget.Texture3D,
                TextureType.CubeMap => TextureTarget.TextureCubeMap,
                _ => throw new ArgumentOutOfRangeException(nameof(textureType))
            };
        }

        public static GLEnum GetTextureWrap(TextureWrapMode textureWrapMode)
        {
            return textureWrapMode switch
            {
                TextureWrapMode.Repeat => GLEnum.Repeat,
                TextureWrapMode.Clamp => GLEnum.ClampToBorder,
                TextureWrapMode.Mirror => GLEnum.MirroredRepeat,
                _ => throw new ArgumentOutOfRangeException(nameof(textureWrapMode))
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
    }
}
