using System;

namespace AvionEngine.Graphics
{
    public abstract class AVPipeline : IDisposable
    {
        public bool IsDisposed { get; protected set; }

        public abstract void SetShader(AVShader shader);

        public abstract void RemoveShader(ShaderStage shaderStage);

        public abstract void Dispose();
    }
}
