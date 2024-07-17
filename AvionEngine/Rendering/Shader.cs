using AvionEngine.Interfaces;
using Silk.NET.Maths;

namespace AvionEngine.Rendering
{
    public class Shader : IShader
    {
        public IShader NativeShader { get; set; } //We can swap out native shaders if we need to.

        public Shader(IShader nativeShader)
        {
            NativeShader = nativeShader;
        }

        public void Render()
        {
            NativeShader.Render();
        }

        public void Reload(string vertexCode, string fragmentCode)
        {
            NativeShader.Reload(vertexCode, fragmentCode);
        }

        public void SetBool(string name, bool value)
        {
            NativeShader.SetBool(name, value);
        }

        public void SetFloat(string name, float value)
        {
            NativeShader.SetFloat(name, value);
        }

        public void SetInt(string name, int value)
        {
            NativeShader.SetInt(name, value);
        }

        public void SetUInt(string name, uint value)
        {
            NativeShader.SetUInt(name, value);
        }

        public void SetUniform2(string name, Matrix2X2<float> value)
        {
            NativeShader.SetUniform2(name, value);
        }

        public void SetUniform3(string name, Matrix3X3<float> value)
        {
            NativeShader.SetUniform3(name, value);
        }

        public void SetUniform4(string name, Matrix4X4<float> value)
        {
            NativeShader.SetUniform4(name, value);
        }
    }
}