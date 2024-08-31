using AvionEngine.Graphics;
using Silk.NET.OpenGL;
using System;

namespace AvionEngine.OpenGL.Graphics
{
    public class GLShaderModule : AVShaderModule
    {
        public readonly Renderer renderer;

        public readonly uint Shader;

        public GLShaderModule(Renderer renderer, string shaderCode, ShaderStage shaderStage) : base(shaderStage)
        {
            this.renderer = renderer;

            Shader = renderer.glContext.CreateShader(GetShaderType(shaderStage));

            renderer.glContext.ShaderSource(Shader, shaderCode);
            Compile(renderer.glContext, Shader);
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                renderer.glContext.DeleteShader(Shader);
            }

            IsDisposed = true;
        }

        ~GLShaderModule()
        {
            Dispose(false);
        }

        public static ShaderType GetShaderType(ShaderStage shaderStage)
        {
            return shaderStage switch
            {
                ShaderStage.Vertex => ShaderType.VertexShader,
                ShaderStage.Geometry => ShaderType.GeometryShader,
                ShaderStage.Pixel => ShaderType.FragmentShader,
                ShaderStage.Compute => ShaderType.ComputeShader,
                _ => throw new ArgumentOutOfRangeException(nameof(shaderStage))
            };
        }

        public static void Compile(GL glInstance, uint shader)
        {
            glInstance.CompileShader(shader);
            glInstance.GetShader(shader, ShaderParameterName.CompileStatus, out var status);
            if (status != (int)GLEnum.True)
                throw new Exception("Shader failed to compile: " + glInstance.GetShaderInfoLog(shader));
        }
    }
}
