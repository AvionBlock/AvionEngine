using Arch.Core;

namespace AvionEngine.Interfaces
{
    public interface IEngine
    {
        IRenderer Renderer { get; }

        World World { get; }
    }
}
