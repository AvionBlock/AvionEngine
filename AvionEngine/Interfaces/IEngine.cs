using Arch.Core;
using AvionEngine.Rendering;

namespace AvionEngine.Interfaces
{
    public interface IEngine
    {
        IRenderer Renderer { get; }

        World World { get; }
    }
}
