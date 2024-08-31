using System;

namespace AvionEngine.Graphics
{
    public abstract class AVShaderModule : IDisposable
    {
        public readonly ShaderStage ShaderStage;

        public bool IsDisposed { get; protected set; }

        public AVShaderModule(ShaderStage shaderStage)
        {
            ShaderStage = shaderStage;
        }

        public abstract void Dispose();
    }
}
