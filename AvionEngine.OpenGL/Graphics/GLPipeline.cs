using AvionEngine.Graphics;
using AvionEngine.Structures;
using Silk.NET.OpenGL;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace AvionEngine.OpenGL.Graphics
{
    public class GLPipeline : AVPipeline
    {
        public readonly Renderer renderer;
        public readonly uint ProgramPipeline;
        public readonly uint VAO;

        public GLPipeline(Renderer renderer) : base()
        {
            this.renderer = renderer;

            ProgramPipeline = renderer.glContext.GenProgramPipeline();

            VAO = renderer.glContext.CreateVertexArray();
            renderer.glContext.BindVertexArray(VAO);

            var verticesFields = typeof(TInput).GetFields().Where(x => Attribute.IsDefined(x, typeof(VertexField))).ToArray();
            for (uint i = 0; i < verticesFields.Length; i++)
            {
                var attribute = verticesFields[i].GetCustomAttribute<VertexField>();
                renderer.glContext.EnableVertexAttribArray(i);

                var offset = (uint)Marshal.OffsetOf<TInput>(verticesFields[i].Name);
                var formatType = attribute.FormatType;
                switch (formatType)
                {
                    case FormatType.R32_Float:
                        renderer.glContext.VertexAttribFormat(i, 1, VertexAttribType.Float, false, offset);
                        break;
                    case FormatType.R32G32_Float:
                        renderer.glContext.VertexAttribFormat(i, 2, VertexAttribType.Float, false, offset);
                        break;
                    case FormatType.R32G32B32_Float:
                        renderer.glContext.VertexAttribFormat(i, 3, VertexAttribType.Float, false, offset);
                        break;
                    case FormatType.R32G32B32A32_Float:
                        renderer.glContext.VertexAttribFormat(i, 4, VertexAttribType.Float, false, offset);
                        break;
                    case FormatType.R8G8B8A8_UNorm:
                        renderer.glContext.VertexAttribFormat(i, 4, VertexAttribType.UnsignedByte, true, offset);
                        break;
                    case FormatType.R8G8B8A8_UNorm_SRGB:
                        renderer.glContext.VertexAttribFormat(i, 4, VertexAttribType.UnsignedByte, true, offset);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(formatType));
                }

                renderer.glContext.VertexBindingDivisor(i, (uint)attribute.InputType);
                renderer.glContext.VertexAttribBinding(i, 0);
            }
        }

        public override void SetShader(AVShader shader)
        {
            if(shader is GLShader glShader)
            {
                renderer.glContext.UseProgramStages(ProgramPipeline, GetProgramStageMask(glShader.ShaderStage), glShader.ShaderProgram);
            }
        }

        public override void RemoveShader(ShaderStage shaderStage)
        {
            renderer.glContext.BindProgramPipeline(ProgramPipeline);
            renderer.glContext.UseProgramStages(ProgramPipeline, GetProgramStageMask(shaderStage), 0);
            renderer.glContext.BindProgramPipeline(0);
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
    }
}
