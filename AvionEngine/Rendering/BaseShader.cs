using AvionEngine.Interfaces;
using Silk.NET.Maths;
using System;

namespace AvionEngine.Rendering
{
    public abstract class BaseShader : IRenderable
    {
        protected IShader? NativeShader { get; set; } //We can swap out native shaders if we need to.

        /// <summary>
        /// Called when a native shader loads the base shader.
        /// </summary>
        /// <param name="nativeShader">The native shader that loaded the base shader.</param>
        /// <param name="vertexCode">The vertex shader code that the native shader uploads to the GPU.</param>
        /// <param name="fragmentCode">The fragment shader code that the native shader uploads to the GPU.</param>
        public abstract void Load(IShader nativeShader, out string vertexCode, out string fragmentCode);

        public virtual void Reload()
        {
            throw new NotImplementedException();
        }

        public virtual void Reload(string vertexCode, string fragmentCode)
        {
            NativeShader?.Reload(vertexCode, fragmentCode);
        }

        public virtual void Render()
        {
            NativeShader?.Render();
        }

        public virtual void SetBool(string name, bool value)
        {
            NativeShader?.SetBool(name, value);
        }

        public virtual void SetFloat(string name, float value)
        {
            NativeShader?.SetFloat(name, value);
        }

        public virtual void SetInt(string name, int value)
        {
            NativeShader?.SetInt(name, value);
        }

        public virtual void SetUInt(string name, uint value)
        {
            NativeShader?.SetUInt(name, value);
        }

        public virtual void SetUniform2(string name, Matrix2X2<float> value)
        {
            NativeShader?.SetUniform2(name, value);
        }

        public virtual void SetUniform3(string name, Matrix3X3<float> value)
        {
            NativeShader?.SetUniform3(name, value);
        }

        public virtual void SetUniform4(string name, Matrix4X4<float> value)
        {
            NativeShader?.SetUniform4(name, value);
        }
    }
}