using AvionEngine.Descriptors;
using AvionEngine.Graphics;
using Silk.NET.OpenGL;
using System;

namespace AvionEngine.OpenGL.Graphics
{
    public class GLPipeline : AVPipeline
    {
        public readonly GLRenderer renderer;
        public readonly uint ProgramPipeline;

        public uint VAO;
        public uint VertexProgram;
        public uint TessCtrlProgram;
        public uint TessEvalProgram;
        public uint GeometryProgram;
        public uint FragmentProgram;
        public uint ComputeProgram;

        public GLPipeline(
            GLRenderer renderer,
            PrimitiveMode primitiveMode,
            in InputLayoutDescriptor[] inputLayoutDescriptors,
            AVShader[]? vertexShaders = null,
            AVShader[]? tessCtrlShaders = null,
            AVShader[]? tessEvalShaders = null,
            AVShader[]? geometryShaders = null,
            AVShader[]? fragmentShaders = null,
            AVShader[]? computeShaders = null) : base(primitiveMode)
        {
            this.renderer = renderer;
            ProgramPipeline = renderer.glContext.GenProgramPipeline();

            SetVertexStage(vertexShaders);
            SetTessCtrlStage(tessCtrlShaders);
            SetTessEvalStage(tessEvalShaders);
            SetGeometryStage(geometryShaders);
            SetPixelStage(fragmentShaders);
            SetComputeStage(computeShaders);
        }

        public override void SetLayoutDescriptors(in InputLayoutDescriptor[] inputLayoutDescriptors)
        {
            VAO = renderer.glContext.CreateVertexArray();
            renderer.glContext.BindVertexArray(VAO);

            for (uint i = 0; i < inputLayoutDescriptors.Length; i++)
            {
                renderer.glContext.EnableVertexAttribArray(i);
                var descriptor = inputLayoutDescriptors[i];
                switch (descriptor.FormatType)
                {
                    case FormatType.R32_Float:
                        renderer.glContext.VertexAttribFormat(i, 1, VertexAttribType.Float, false, descriptor.Offset);
                        break;
                    case FormatType.R32G32_Float:
                        renderer.glContext.VertexAttribFormat(i, 2, VertexAttribType.Float, false, descriptor.Offset);
                        break;
                    case FormatType.R32G32B32_Float:
                        renderer.glContext.VertexAttribFormat(i, 3, VertexAttribType.Float, false, descriptor.Offset);
                        break;
                    case FormatType.R32G32B32A32_Float:
                        renderer.glContext.VertexAttribFormat(i, 4, VertexAttribType.Float, false, descriptor.Offset);
                        break;
                    case FormatType.R8G8B8A8_UNorm:
                        renderer.glContext.VertexAttribFormat(i, 4, VertexAttribType.UnsignedByte, true, descriptor.Offset);
                        break;
                    case FormatType.R8G8B8A8_UNorm_SRGB:
                        renderer.glContext.VertexAttribFormat(i, 4, VertexAttribType.UnsignedByte, true, descriptor.Offset);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(descriptor.FormatType));
                }

                renderer.glContext.VertexBindingDivisor(i, (uint)descriptor.InputType);
                renderer.glContext.VertexAttribBinding(i, 0);
            }

            //Unbind for safety.
            renderer.glContext.BindVertexArray(0);
        }

        public override void SetVertexStage(AVShader[]? shaders = null)
        {
            if (shaders == null && VertexProgram != 0)
            {
                renderer.glContext.UseProgramStages(ProgramPipeline, UseProgramStageMask.VertexShaderBit, 0);
                renderer.glContext.DeleteProgram(VertexProgram);
                VertexProgram = 0;
                return;
            }
            else if (shaders == null)
                return;

            var glShaders = (GLShader[])shaders;
            if (glShaders == null)
                throw new ArgumentException($"Shaders must be of type {nameof(GLShader)}.");
            var program = CreateShaderProgram(renderer.glContext, ShaderStage.Vertex, glShaders);

            renderer.glContext.UseProgramStages(ProgramPipeline, UseProgramStageMask.VertexShaderBit, program);
            if (VertexProgram != 0)
                renderer.glContext.DeleteProgram(VertexProgram);
            VertexProgram = program;
        }

        public override void SetTessCtrlStage(AVShader[]? shaders = null)
        {
            if (shaders == null && TessCtrlProgram != 0)
            {
                renderer.glContext.UseProgramStages(ProgramPipeline, UseProgramStageMask.TessControlShaderBit, 0);
                renderer.glContext.DeleteProgram(TessCtrlProgram);
                TessCtrlProgram = 0;
                return;
            }
            else if (shaders == null)
                return;

            var glShaders = (GLShader[])shaders;
            if (glShaders == null)
                throw new ArgumentException($"Shaders must be of type {nameof(GLShader)}.");
            var program = CreateShaderProgram(renderer.glContext, ShaderStage.TessCtrl, glShaders);

            renderer.glContext.UseProgramStages(ProgramPipeline, UseProgramStageMask.TessControlShaderBit, program);
            if (TessCtrlProgram != 0)
                renderer.glContext.DeleteProgram(TessCtrlProgram);
            TessCtrlProgram = program;
        }

        public override void SetTessEvalStage(AVShader[]? shaders = null)
        {
            if (shaders == null && TessEvalProgram != 0)
            {
                renderer.glContext.UseProgramStages(ProgramPipeline, UseProgramStageMask.TessEvaluationShaderBit, 0);
                renderer.glContext.DeleteProgram(TessEvalProgram);
                TessEvalProgram = 0;
                return;
            }
            else if (shaders == null)
                return;

            var glShaders = (GLShader[])shaders;
            if (glShaders == null)
                throw new ArgumentException($"Shaders must be of type {nameof(GLShader)}.");
            var program = CreateShaderProgram(renderer.glContext, ShaderStage.TessEval, glShaders);

            renderer.glContext.UseProgramStages(ProgramPipeline, UseProgramStageMask.TessEvaluationShaderBit, program);
            if (TessEvalProgram != 0)
                renderer.glContext.DeleteProgram(TessEvalProgram);
            TessEvalProgram = program;
        }

        public override void SetGeometryStage(AVShader[]? shaders = null)
        {
            if (shaders == null && GeometryProgram != 0)
            {
                renderer.glContext.UseProgramStages(ProgramPipeline, UseProgramStageMask.GeometryShaderBit, 0);
                renderer.glContext.DeleteProgram(GeometryProgram);
                GeometryProgram = 0;
                return;
            }
            else if (shaders == null)
                return;

            var glShaders = (GLShader[])shaders;
            if (glShaders == null)
                throw new ArgumentException($"Shaders must be of type {nameof(GLShader)}.");
            var program = CreateShaderProgram(renderer.glContext, ShaderStage.Geometry, glShaders);

            renderer.glContext.UseProgramStages(ProgramPipeline, UseProgramStageMask.GeometryShaderBit, program);
            if (GeometryProgram != 0)
                renderer.glContext.DeleteProgram(GeometryProgram);
            GeometryProgram = program;
        }

        public override void SetPixelStage(AVShader[]? shaders = null)
        {
            if (shaders == null && FragmentProgram != 0)
            {
                renderer.glContext.UseProgramStages(ProgramPipeline, UseProgramStageMask.FragmentShaderBit, 0);
                renderer.glContext.DeleteProgram(FragmentProgram);
                FragmentProgram = 0;
                return;
            }
            else if (shaders == null)
                return;

            var glShaders = (GLShader[])shaders;
            if (glShaders == null)
                throw new ArgumentException($"Shaders must be of type {nameof(GLShader)}.");
            var program = CreateShaderProgram(renderer.glContext, ShaderStage.Pixel, glShaders);

            renderer.glContext.UseProgramStages(ProgramPipeline, UseProgramStageMask.FragmentShaderBit, program);
            if (FragmentProgram != 0)
                renderer.glContext.DeleteProgram(FragmentProgram);
            FragmentProgram = program;
        }

        public override void SetComputeStage(AVShader[]? shaders = null)
        {
            if (shaders == null && ComputeProgram != 0)
            {
                renderer.glContext.UseProgramStages(ProgramPipeline, UseProgramStageMask.ComputeShaderBit, 0);
                renderer.glContext.DeleteProgram(ComputeProgram);
                ComputeProgram = 0;
                return;
            }
            else if (shaders == null)
                return;

            var glShaders = (GLShader[])shaders;
            if (glShaders == null)
                throw new ArgumentException($"Shaders must be of type {nameof(GLShader)}.");
            var program = CreateShaderProgram(renderer.glContext, ShaderStage.Compute, glShaders);

            renderer.glContext.UseProgramStages(ProgramPipeline, UseProgramStageMask.ComputeShaderBit, program);
            if (ComputeProgram != 0)
                renderer.glContext.DeleteProgram(ComputeProgram);
            ComputeProgram = program;
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
                renderer.glContext.DeleteProgramPipeline(ProgramPipeline);
            }

            IsDisposed = true;
        }

        ~GLPipeline()
        {
            Dispose(false);
        }

        public static UseProgramStageMask GetProgramStageMask(ShaderStage shaderStage)
        {
            return shaderStage switch
            {
                ShaderStage.Vertex => UseProgramStageMask.VertexShaderBit,
                ShaderStage.TessCtrl => UseProgramStageMask.TessControlShaderBit,
                ShaderStage.TessEval => UseProgramStageMask.TessEvaluationShaderBit,
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

        public static uint CreateShaderProgram(GL gl, ShaderStage shaderStage, GLShader[] shaders)
        {
            uint program = gl.CreateProgram();
            gl.ProgramParameter(program, ProgramParameterPName.Separable, (int)GLEnum.True);

            foreach (var shader in shaders)
            {
                if (shader.ShaderStage != shaderStage)
                {
                    gl.DeleteProgram(program);
                    throw new Exception($"Unexpected shader stage, expected: {shaderStage}, got: {shader.ShaderStage}.");
                }
                gl.AttachShader(program, shader.Shader);
            }

            gl.LinkProgram(program);
            gl.GetProgram(program, ProgramPropertyARB.LinkStatus, out int status);
            if (status != (int)GLEnum.True)
                throw new Exception($"Failed to link program: {gl.GetProgramInfoLog(program)}");

            foreach (var shader in shaders)
            {
                gl.DetachShader(program, shader.Shader);
            }

            return program;
        }
    }
}
