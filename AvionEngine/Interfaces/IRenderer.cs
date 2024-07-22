using AvionEngine.Rendering;
using Silk.NET.Windowing;
using System.Drawing;

namespace AvionEngine.Interfaces
{
    public interface IRenderer
    {
        IWindow Window { get; }

        Color ClearColor { get; set; }

        IShader CreateShader(BaseShader shader);

        void Clear();
    }
}
