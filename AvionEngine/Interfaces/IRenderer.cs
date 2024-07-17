using Silk.NET.Windowing;
using System.Drawing;

namespace AvionEngine.Interfaces
{
    public interface IRenderer
    {
        Color ClearColor { get; set; }

        IWindow Window { get; }

        IShader CreateShader(string vertexCode, string fragmentCode);

        void Clear();
    }
}
