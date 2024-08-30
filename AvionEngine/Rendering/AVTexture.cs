using AvionEngine.Structures;
using System;
using System.Runtime.CompilerServices;

namespace AvionEngine.Rendering
{
    public abstract class AVTexture : IDisposable
    {
        public readonly TextureType TextureType;
        public readonly TextureFormatMode TextureFormatMode;

        public bool IsDisposed { get; protected set; }

        public AVTexture(TextureType textureType, TextureFormatMode textureFormatMode)
        {
            TextureType = textureType;
            TextureFormatMode = textureFormatMode;
        }

        public abstract void Update(TextureInfo textureInfo);

        public abstract void Dispose();

        //https://github.com/Aquatic-Games/grabs/blob/main/src/grabs.Graphics/GraphicsUtils.cs
        public static uint BitsPerPixel(TextureFormatMode format)
        {
            switch (format)
            {
                case TextureFormatMode.R8_UNorm:
                case TextureFormatMode.R8_UInt:
                case TextureFormatMode.R8_SNorm:
                case TextureFormatMode.R8_SInt:
                case TextureFormatMode.A8_UNorm:
                    return 8;

                case TextureFormatMode.R8G8_UNorm:
                case TextureFormatMode.R8G8_UInt:
                case TextureFormatMode.R8G8_SNorm:
                case TextureFormatMode.R8G8_SInt:
                    return 16;

                case TextureFormatMode.R16_Float:
                case TextureFormatMode.D16_UNorm:
                case TextureFormatMode.R16_UNorm:
                case TextureFormatMode.R16_UInt:
                case TextureFormatMode.R16_SNorm:
                case TextureFormatMode.R16_SInt:
                    return 16;

                case TextureFormatMode.B5G6R5_UNorm:
                case TextureFormatMode.B5G5R5A1_UNorm:
                    return 16;

                case TextureFormatMode.R10G10B10A2_UNorm:
                case TextureFormatMode.R10G10B10A2_UInt:
                case TextureFormatMode.R11G11B10_Float:
                    return 32;

                case TextureFormatMode.R8G8B8A8_UNorm:
                case TextureFormatMode.R8G8B8A8_UNorm_SRGB:
                case TextureFormatMode.R8G8B8A8_UInt:
                case TextureFormatMode.R8G8B8A8_SNorm:
                case TextureFormatMode.R8G8B8A8_SInt:
                case TextureFormatMode.B8G8R8A8_UNorm:
                case TextureFormatMode.B8G8R8A8_UNorm_SRGB:
                    return 32;

                case TextureFormatMode.R16G16_Float:
                case TextureFormatMode.R16G16_UNorm:
                case TextureFormatMode.R16G16_UInt:
                case TextureFormatMode.R16G16_SNorm:
                case TextureFormatMode.R16G16_SInt:
                    return 32;

                case TextureFormatMode.R32_Float:
                case TextureFormatMode.R32_UInt:
                case TextureFormatMode.R32_SInt:
                    return 32;

                case TextureFormatMode.D24_UNorm_S8_UInt:
                case TextureFormatMode.D32_Float:
                    return 32;

                case TextureFormatMode.R16G16B16A16_Float:
                case TextureFormatMode.R16G16B16A16_UNorm:
                case TextureFormatMode.R16G16B16A16_UInt:
                case TextureFormatMode.R16G16B16A16_SNorm:
                case TextureFormatMode.R16G16B16A16_SInt:
                    return 64;


                case TextureFormatMode.R32G32_Float:
                case TextureFormatMode.R32G32_UInt:
                case TextureFormatMode.R32G32_SInt:
                    return 64;

                case TextureFormatMode.R32G32B32_Float:
                case TextureFormatMode.R32G32B32_UInt:
                case TextureFormatMode.R32G32B32_SInt:
                    return 96;

                case TextureFormatMode.R32G32B32A32_Float:
                case TextureFormatMode.R32G32B32A32_UInt:
                case TextureFormatMode.R32G32B32A32_SInt:
                    return 128;

                case TextureFormatMode.BC1_UNorm:
                case TextureFormatMode.BC1_UNorm_SRGB:
                case TextureFormatMode.BC4_UNorm:
                case TextureFormatMode.BC4_SNorm:
                    return 4;

                case TextureFormatMode.BC2_UNorm:
                case TextureFormatMode.BC2_UNorm_SRGB:
                case TextureFormatMode.BC3_UNorm:
                case TextureFormatMode.BC3_UNorm_SRGB:
                case TextureFormatMode.BC5_UNorm:
                case TextureFormatMode.BC5_SNorm:
                case TextureFormatMode.BC6H_UF16:
                case TextureFormatMode.BC6H_SF16:
                case TextureFormatMode.BC7_UNorm:
                case TextureFormatMode.BC7_UNorm_SRGB:
                    return 8;

                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FormatIsCompressed(TextureFormatMode textureFormatMode) => (int)textureFormatMode >= (int)TextureFormatMode.BC1_UNorm && (int)textureFormatMode <= (int)TextureFormatMode.BC7_UNorm_SRGB;

        public static uint CalculatePitch(TextureFormatMode textureFormatMode, uint width)
        {
            if (FormatIsCompressed(textureFormatMode))
            {
                uint blockSize = 0;
                switch (textureFormatMode)
                {
                    case TextureFormatMode.BC1_UNorm:
                    case TextureFormatMode.BC1_UNorm_SRGB:
                    case TextureFormatMode.BC4_UNorm:
                    case TextureFormatMode.BC4_SNorm:
                        blockSize = 8;
                        break;

                    case TextureFormatMode.BC2_UNorm:
                    case TextureFormatMode.BC2_UNorm_SRGB:
                    case TextureFormatMode.BC3_UNorm:
                    case TextureFormatMode.BC3_UNorm_SRGB:
                    case TextureFormatMode.BC5_UNorm:
                    case TextureFormatMode.BC5_SNorm:
                    case TextureFormatMode.BC6H_UF16:
                    case TextureFormatMode.BC6H_SF16:
                    case TextureFormatMode.BC7_UNorm:
                    case TextureFormatMode.BC7_UNorm_SRGB:
                        blockSize = 16;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(textureFormatMode), textureFormatMode, null);
                }

                return Math.Max(1, ((width + 3) >> 2)) * blockSize;
            }

            uint bpp = BitsPerPixel(textureFormatMode);
            return (width * bpp + 7) >> 3;
        }

        public static uint CalculateTextureSizeInBytes(TextureFormatMode textureFormatMode, uint width, uint height)
        {
            if (FormatIsCompressed(textureFormatMode))
                return Math.Max(1, (width + 3) >> 2) * Math.Max(1, (height + 3) >> 2) * BitsPerPixel(textureFormatMode) * 2;

            return CalculatePitch(textureFormatMode, width) * height;
        }
    }
}
