using AvionEngine.Descriptors;
using System;

namespace AvionEngine.Graphics
{
    public abstract class AVPipeline : IDisposable
    {
        public readonly PrimitiveMode PrimitiveMode;

        public bool IsDisposed { get; protected set; }

        public AVPipeline(PrimitiveMode primitiveMode)
        {
            PrimitiveMode = primitiveMode;
        }

        public abstract void SetLayoutDescriptors(in InputLayoutDescriptor[] inputLayoutDescriptors);

        public abstract void SetVertexStage(AVShader[]? shaders = null);

        public abstract void SetTessCtrlStage(AVShader[]? shaders = null);

        public abstract void SetTessEvalStage(AVShader[]? shaders = null);

        public abstract void SetGeometryStage(AVShader[]? shaders = null);

        public abstract void SetPixelStage(AVShader[]? shaders = null);

        public abstract void SetComputeStage(AVShader[]? shaders = null);

        public abstract void Dispose();
    }
}
