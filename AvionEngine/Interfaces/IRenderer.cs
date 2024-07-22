using AvionEngine.Rendering;
using Silk.NET.Windowing;
using System.Collections.Generic;
using System.Drawing;

namespace AvionEngine.Interfaces
{
    public interface IRenderer
    {
        IWindow Window { get; }

        Color ClearColor { get; set; }

        List<IShader> Shaders { get; set; }

        void AddShader(BaseShader shader);

        void RemoveShader(BaseShader shader);

        void Clear();
    }
}
