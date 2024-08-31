using AvionEngine.Graphics;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System;

namespace AvionEngine.OpenGL.Graphics
{
    public class GLShader : AVShader
    {
        public readonly Renderer renderer;

        public uint ShaderProgram { get; private set; }

        public unsafe GLShader(Renderer renderer, ShaderStage shaderStage, params AVShaderModule[] shaderModules) : base()
        {
            this.renderer = renderer;

            Reload(shaderStage, shaderModules);
        }

        public override void Reload(ShaderStage shaderStage, params AVShaderModule[] shaderModules)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            ShaderStage = shaderStage;

            foreach (var shaderModule in shaderModules)
                if (shaderModule.ShaderStage != ShaderStage)
                    throw new ArgumentException($"All shader modules must be the same as the {nameof(ShaderStage)}", nameof(shaderModules));  

            if (shaderModules is GLShaderModule[] glShaderModules)
            {
                if (ShaderProgram != 0)
                {
                    renderer.glContext.DeleteProgram(ShaderProgram);
                    ShaderProgram = 0;
                }
                ShaderProgram = renderer.glContext.CreateProgram();
                renderer.glContext.ProgramParameter(ShaderProgram, ProgramParameterPName.Separable, (int)GLEnum.True);

                foreach (var glShaderModule in glShaderModules)
                {
                    renderer.glContext.AttachShader(ShaderProgram, glShaderModule.Shader);
                }

                renderer.glContext.LinkProgram(ShaderProgram);
                renderer.glContext.GetProgram(ShaderProgram, ProgramPropertyARB.LinkStatus, out int status);
                if (status != (int)GLEnum.True)
                    throw new Exception($"Failed to link program: {renderer.glContext.GetProgramInfoLog(ShaderProgram)}");

                foreach (var glShaderModule in glShaderModules)
                {
                    renderer.glContext.DetachShader(ShaderProgram, glShaderModule.Shader);
                }
            }
        }

        public override void SetBool(string name, bool value)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            renderer.glContext.Uniform1(renderer.glContext.GetUniformLocation(ShaderProgram, name), value ? 1 : 0);
        }

        public override void SetInt(string name, int value)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            renderer.glContext.Uniform1(renderer.glContext.GetUniformLocation(ShaderProgram, name), value);
        }

        public override void SetUInt(string name, uint value)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            renderer.glContext.Uniform1(renderer.glContext.GetUniformLocation(ShaderProgram, name), value);
        }

        public override void SetFloat(string name, float value)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            renderer.glContext.Uniform1(renderer.glContext.GetUniformLocation(ShaderProgram, name), value);
        }

        public override unsafe void SetUniform2(string name, Matrix2X2<float> value)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            renderer.glContext.UniformMatrix2(renderer.glContext.GetUniformLocation(ShaderProgram, name), 1, true, (float*)&value);
        }

        public override unsafe void SetUniform3(string name, Matrix3X3<float> value)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            renderer.glContext.UniformMatrix3(renderer.glContext.GetUniformLocation(ShaderProgram, name), 1, true, (float*)&value);
        }   

        public override unsafe void SetUniform4(string name, Matrix4X4<float> value)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            renderer.glContext.UniformMatrix4(renderer.glContext.GetUniformLocation(ShaderProgram, name), 1, true, (float*)&value);
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
                renderer.glContext.DeleteProgram(ShaderProgram);
            }

            IsDisposed = true;
        }

        ~GLShader()
        {
            Dispose(false);
        }

        public static UseProgramStageMask GetProgramStageMask(ShaderStage shaderStage)
        {
            return shaderStage switch
            {
                ShaderStage.Vertex => UseProgramStageMask.VertexShaderBit,
                ShaderStage.Geometry => UseProgramStageMask.GeometryShaderBit,
                ShaderStage.Pixel => UseProgramStageMask.FragmentShaderBit,
                ShaderStage.Compute => UseProgramStageMask.ComputeShaderBit,
                _ => throw new ArgumentOutOfRangeException(nameof(shaderStage)),
            };
        }

        public static PrimitiveType GetPrimitiveType(PrimitiveMode primitiveMode)
        {
            return primitiveMode switch
            {
                PrimitiveMode.PointList => PrimitiveType.Points,
                PrimitiveMode.LineList => PrimitiveType.Lines,
                PrimitiveMode.LineStrip => PrimitiveType.LineStrip,
                PrimitiveMode.LineListAdjacency => PrimitiveType.LinesAdjacency,
                PrimitiveMode.LineStripAdjacency => PrimitiveType.LineStripAdjacency,
                PrimitiveMode.TriangleList => PrimitiveType.Triangles,
                PrimitiveMode.TriangleStrip => PrimitiveType.TriangleStrip,
                PrimitiveMode.TriangleFan => PrimitiveType.TriangleFan,
                PrimitiveMode.TriangleListAdjacency => PrimitiveType.TrianglesAdjacency,
                PrimitiveMode.TriangleStripAdjacency => PrimitiveType.TriangleStripAdjacency,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
