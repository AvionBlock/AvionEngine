using AvionEngine.Enums;

namespace AvionEngine.Interfaces
{
    public interface ITexture
    {
        void Assign();

        void Render();

        void Set(uint width, uint height, byte[] data, TextureFormat format = TextureFormat.RGB);
    }
}
