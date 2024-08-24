using AvionEngine.Interfaces;
using Silk.NET.Maths;
using System;

namespace AvionEngine.Rendering
{
    public abstract class BaseShader : IVisual, IDisposable
    {
        public abstract IRenderer Renderer { get; }

        public bool IsDisposed { get; protected set; }

        public abstract void Render(double delta);

        public abstract void Reload(string vertexCode, string fragmentCode);

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