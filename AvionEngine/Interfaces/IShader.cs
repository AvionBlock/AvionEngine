using Silk.NET.Maths;

namespace AvionEngine.Interfaces
{
    public interface IShader
    {
        void Reload(string vertexCode, string fragmentCode);

        void SetBool(string name, bool value);

        void SetInt(string name, int value);

        void SetUInt(string name, uint value);

        void SetFloat(string name, float value);

        void SetUniform2(string name, Matrix2X2<float> value);

        void SetUniform3(string name, Matrix3X3<float> value);

        void SetUniform4(string name, Matrix4X4<float> value);

        void Bind();
    }
}
