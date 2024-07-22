using AvionEngine.Rendering;
using Silk.NET.Windowing;
using System.Collections.Generic;
using System.Drawing;

namespace AvionEngine.Interfaces
{
    public interface IRenderer
    {
        IWindow Window { get; }

        IEnumerable<IShader> Shaders { get; }

        Color ClearColor { get; set; }

        void AddShader(BaseShader shader);

        bool RemoveShader(BaseShader shader);

        void ClearShaders();

        void Clear();
    }
}
