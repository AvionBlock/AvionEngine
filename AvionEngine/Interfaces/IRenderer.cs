using System.Collections.Generic;

namespace AvionEngine.Interfaces
{
    public interface IRenderer
    {
        List<IShader> Shaders { get; set; }
        List<EngineObject> Objects { get; set; }

        IShader CreateShader(string vertexCode, string fragmentCode);

        void Draw(uint indicesLength);
    }
}
