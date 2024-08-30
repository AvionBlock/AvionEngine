using System;

namespace AvionEngine.Rendering
{
    public abstract class AVShader : IDisposable
    {
        public readonly ShaderStage ShaderStage;

        public bool IsDisposed { get; protected set; }

        public AVShader(ShaderStage shaderStage)
        {
            ShaderStage = shaderStage;
        }

        public abstract void Dispose();
    }
}
