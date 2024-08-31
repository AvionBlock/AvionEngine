using Silk.NET.Maths;
using System;

namespace AvionEngine.Graphics
{
    public abstract class AVShader : IDisposable
    {
        public ShaderStage ShaderStage { get; protected set; }
        public bool IsDisposed { get; protected set; }

        public abstract void Reload(ShaderStage shaderStage, params AVShaderModule[] shaderModules);

        public abstract void SetBool(string name, bool value);

        public abstract void SetInt(string name, int value);

        public abstract void SetUInt(string name, uint value);

        public abstract void SetFloat(string name, float value);

        public abstract void SetUniform2(string name, Matrix2X2<float> value);

        public abstract void SetUniform3(string name, Matrix3X3<float> value);

        public abstract void SetUniform4(string name, Matrix4X4<float> value);

        public abstract void Dispose();
    }
}
