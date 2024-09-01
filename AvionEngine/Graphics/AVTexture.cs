using AvionEngine.Descriptors;
using System;
using System.Runtime.CompilerServices;

namespace AvionEngine.Graphics
{
    public abstract class AVTexture : IDisposable
    {
        public readonly TextureType TextureType;
        public readonly FormatType FormatType;
        public TextureWrapMode TextureWrapModeS { get; protected set; }
        public TextureWrapMode TextureWrapModeT { get; protected set; }
        public TextureWrapMode TextureWrapModeR { get; protected set; }
        public MinFilterMode MinFilterMode { get; protected set; }
        public MagFilterMode MagFilterMode { get; protected set; }

        public bool IsDisposed { get; protected set; }

        public AVTexture(
            TextureType textureType,
            FormatType formatType,
            TextureWrapMode textureWrapModeS = TextureWrapMode.Repeat,
            TextureWrapMode textureWrapModeT = TextureWrapMode.Repeat,
            TextureWrapMode textureWrapModeR = TextureWrapMode.Repeat,
            MinFilterMode minFilterMode = MinFilterMode.Linear,
            MagFilterMode magFilterMode = MagFilterMode.Linear)
        {
            TextureType = textureType;
            FormatType = formatType;

            SetWrapMode(textureWrapModeS, textureWrapModeT, textureWrapModeR);
            SetFilterMode(minFilterMode, magFilterMode);
        }

        public abstract void Update(TextureDescriptor textureInfo);

        public virtual void SetWrapMode(TextureWrapMode? textureWrapModeS = null, TextureWrapMode? textureWrapModeT = null, TextureWrapMode? textureWrapModeR = null)
        {
            TextureWrapModeS = textureWrapModeS ?? TextureWrapModeS;
            TextureWrapModeT = textureWrapModeT ?? TextureWrapModeT;
            TextureWrapModeR = textureWrapModeR ?? TextureWrapModeR;
        }

        public virtual void SetFilterMode(MinFilterMode? minFilterMode = null, MagFilterMode? magFilterMode = null)
        {
            MinFilterMode = minFilterMode ?? MinFilterMode;
            MagFilterMode = magFilterMode ?? MagFilterMode;
        }

        public abstract void Dispose();

        //https://github.com/Aquatic-Games/grabs/blob/main/src/grabs.Graphics/GraphicsUtils.cs
        public static uint BitsPerPixel(FormatType formatType)
        {
            switch (formatType)
            {
                case FormatType.R8_UNorm:
                case FormatType.R8_UInt:
                case FormatType.R8_SNorm:
                case FormatType.R8_SInt:
                case FormatType.A8_UNorm:
                    return 8;

                case FormatType.R8G8_UNorm:
                case FormatType.R8G8_UInt:
                case FormatType.R8G8_SNorm:
                case FormatType.R8G8_SInt:
                    return 16;

                case FormatType.R16_Float:
                case FormatType.D16_UNorm:
                case FormatType.R16_UNorm:
                case FormatType.R16_UInt:
                case FormatType.R16_SNorm:
                case FormatType.R16_SInt:
                    return 16;

                case FormatType.B5G6R5_UNorm:
                case FormatType.B5G5R5A1_UNorm:
                    return 16;

                case FormatType.R10G10B10A2_UNorm:
                case FormatType.R10G10B10A2_UInt:
                case FormatType.R11G11B10_Float:
                    return 32;

                case FormatType.R8G8B8A8_UNorm:
                case FormatType.R8G8B8A8_UNorm_SRGB:
                case FormatType.R8G8B8A8_UInt:
                case FormatType.R8G8B8A8_SNorm:
                case FormatType.R8G8B8A8_SInt:
                case FormatType.B8G8R8A8_UNorm:
                case FormatType.B8G8R8A8_UNorm_SRGB:
                    return 32;

                case FormatType.R16G16_Float:
                case FormatType.R16G16_UNorm:
                case FormatType.R16G16_UInt:
                case FormatType.R16G16_SNorm:
                case FormatType.R16G16_SInt:
                    return 32;

                case FormatType.R32_Float:
                case FormatType.R32_UInt:
                case FormatType.R32_SInt:
                    return 32;

                case FormatType.D24_UNorm_S8_UInt:
                case FormatType.D32_Float:
                    return 32;

                case FormatType.R16G16B16A16_Float:
                case FormatType.R16G16B16A16_UNorm:
                case FormatType.R16G16B16A16_UInt:
                case FormatType.R16G16B16A16_SNorm:
                case FormatType.R16G16B16A16_SInt:
                    return 64;


                case FormatType.R32G32_Float:
                case FormatType.R32G32_UInt:
                case FormatType.R32G32_SInt:
                    return 64;

                case FormatType.R32G32B32_Float:
                case FormatType.R32G32B32_UInt:
                case FormatType.R32G32B32_SInt:
                    return 96;

                case FormatType.R32G32B32A32_Float:
                case FormatType.R32G32B32A32_UInt:
                case FormatType.R32G32B32A32_SInt:
                    return 128;

                case FormatType.BC1_UNorm:
                case FormatType.BC1_UNorm_SRGB:
                case FormatType.BC4_UNorm:
                case FormatType.BC4_SNorm:
                    return 4;

                case FormatType.BC2_UNorm:
                case FormatType.BC2_UNorm_SRGB:
                case FormatType.BC3_UNorm:
                case FormatType.BC3_UNorm_SRGB:
                case FormatType.BC5_UNorm:
                case FormatType.BC5_SNorm:
                case FormatType.BC6H_UF16:
                case FormatType.BC6H_SF16:
                case FormatType.BC7_UNorm:
                case FormatType.BC7_UNorm_SRGB:
                    return 8;

                default:
                    throw new ArgumentOutOfRangeException(nameof(formatType), formatType, null);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FormatIsCompressed(FormatType formatType) => (int)formatType >= (int)FormatType.BC1_UNorm && (int)formatType <= (int)FormatType.BC7_UNorm_SRGB;

        public static uint CalculatePitch(FormatType formatType, uint width)
        {
            if (FormatIsCompressed(formatType))
            {
                uint blockSize = 0;
                switch (formatType)
                {
                    case FormatType.BC1_UNorm:
                    case FormatType.BC1_UNorm_SRGB:
                    case FormatType.BC4_UNorm:
                    case FormatType.BC4_SNorm:
                        blockSize = 8;
                        break;

                    case FormatType.BC2_UNorm:
                    case FormatType.BC2_UNorm_SRGB:
                    case FormatType.BC3_UNorm:
                    case FormatType.BC3_UNorm_SRGB:
                    case FormatType.BC5_UNorm:
                    case FormatType.BC5_SNorm:
                    case FormatType.BC6H_UF16:
                    case FormatType.BC6H_SF16:
                    case FormatType.BC7_UNorm:
                    case FormatType.BC7_UNorm_SRGB:
                        blockSize = 16;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(formatType), formatType, null);
                }

                return Math.Max(1, width + 3 >> 2) * blockSize;
            }

            uint bpp = BitsPerPixel(formatType);
            return width * bpp + 7 >> 3;
        }

        public static uint CalculateTextureSizeInBytes(FormatType formatType, uint width, uint height)
        {
            if (FormatIsCompressed(formatType))
                return Math.Max(1, width + 3 >> 2) * Math.Max(1, height + 3 >> 2) * BitsPerPixel(formatType) * 2;

            return CalculatePitch(formatType, width) * height;
        }
    }
}
