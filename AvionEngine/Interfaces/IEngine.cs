using AvionEngine.Rendering;

namespace AvionEngine.Interfaces
{
    public interface IEngine
    {
        IRenderer Renderer { get; }

        void AddShader(BaseShader baseShader);

        void RemoveShader(BaseShader baseShader);
    }
}
