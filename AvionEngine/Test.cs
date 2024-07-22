using AvionEngine.Interfaces;
using AvionEngine.Rendering;

namespace AvionEngine
{
    internal class Test : BaseShader
    {
        public override void Load(IShader nativeShader, out string vertexCode, out string fragmentCode)
        {
            NativeShader = nativeShader;
            NativeShader.Render();
        }
    }
}
