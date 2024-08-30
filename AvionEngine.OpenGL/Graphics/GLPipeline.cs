using AvionEngine.Graphics;
using AvionEngine.Structures;
using Silk.NET.OpenGL;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace AvionEngine.OpenGL.Graphics
{
    public class GLPipeline<TInput> : AVPipeline where TInput : unmanaged
    {
        public readonly Renderer renderer;

        public readonly uint ProgramPipeline;

        public unsafe GLPipeline(Renderer renderer)
        {
            this.renderer = renderer;

            ProgramPipeline = renderer.glContext.GenProgramPipeline();
            var verticesFields = typeof(TInput).GetFields().Where(x => Attribute.IsDefined(x, typeof(VertexField))).ToArray();

            for (uint i = 0; i < verticesFields.Length; i++)
            {
                var fieldSize = verticesFields[i].FieldType.GetFields().Length;
                if (fieldSize <= 0)
                    fieldSize = 1;

                renderer.glContext.EnableVertexAttribArray(i);
                renderer.glContext.VertexAttribFormat(
                    i,
                    fieldSize,
                    GetVertexAttribPointerType(verticesFields[i].GetCustomAttribute<VertexField>().FieldType),
                    false,
                    (uint)sizeof(TInput),
                    (void*)Marshal.OffsetOf<TInput>(verticesFields[i].Name));
            }
        }

        public override void AddShader(AVShader shader)
        {
            if (shader is GLShader glShader)
            {
                var program = CreateProgram(renderer.glContext, glShader);
                Shaders.Add(program); //Keep track of the program.
                renderer.glContext.UseProgramStages(ProgramPipeline, GetProgramStageMask(glShader.ShaderStage), program);
            }
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
                ShaderStage.Geometry => UseProgramStageMask.GeometryShaderBit,
                ShaderStage.Pixel => UseProgramStageMask.FragmentShaderBit,
                ShaderStage.Compute => UseProgramStageMask.ComputeShaderBit,
                _ => throw new ArgumentOutOfRangeException(nameof(shaderStage)),
            };
        }

        public static void SetVertexAttribFormat(GL glInstance, uint index, FormatType formatType, uint offset)
        {
            switch (formatType)
            {
                case FormatType.B5G6R5_UNorm:
                    glInstance.VertexAttribFormat(index, 3, VertexAttribType.UnsignedInt, true, offset);
                    break;
                case FormatType.B5G5R5A1_UNorm:
                    glInstance.VertexAttribFormat(index, 4, VertexAttribType.UnsignedInt, true, offset);
                    break;
                case FormatType.R8_UNorm:
                    glInstance.VertexAttribFormat(index, 1, VertexAttribType.UnsignedInt, true, offset);
                    break;
                case FormatType.R8_UInt:
                    glInstance.VertexAttribFormat(index, 1, VertexAttribType.UnsignedInt, false, offset);
                    break;
                case FormatType.R8_SNorm:
                    glInstance.VertexAttribFormat(index, 1, VertexAttribType.Int, true, offset);
                    break;
                case FormatType.R8_SInt:
                    glInstance.VertexAttribFormat(index, 1, VertexAttribType.Int, false, offset);
                    break;
                case FormatType.A8_UNorm:
                    glInstance.VertexAttribFormat(index, 1, VertexAttribType.UnsignedInt, true, offset);
                    break;
                case FormatType.R8G8_UNorm:
                    glInstance.VertexAttribFormat(index, 2, VertexAttribType.UnsignedInt, true, offset);
                    break;
                case FormatType.R8G8_UInt:
                    glInstance.VertexAttribFormat(index, 2, VertexAttribType.UnsignedInt, false, offset);
                    break;
                case FormatType.R8G8_SNorm:
                    glInstance.VertexAttribFormat(index, 2, VertexAttribType.Int, true, offset);
                    break;
                case FormatType.R8G8_SInt:
                    glInstance.VertexAttribFormat(index, 2, VertexAttribType.Int, false, offset);
                    break;
                case FormatType.R8G8B8A8_UNorm:
                    glInstance.VertexAttribFormat(index, 4, VertexAttribType.UnsignedByte, true, offset); //Dunno.
                    //glInstance.VertexAttribFormat(index, 4, VertexAttribType.UnsignedInt, true, offset);
                    break;
                case FormatType.R8G8B8A8_UNorm_SRGB:
                    glInstance.VertexAttribFormat(index, 4, VertexAttribType.UnsignedInt, true, offset);
                    break;
                case FormatType.R8G8B8A8_UInt:
                    glInstance.VertexAttribFormat(index, 4, VertexAttribType.UnsignedInt, false, offset);
                    break;
                case FormatType.R8G8B8A8_SNorm:
                    glInstance.VertexAttribFormat(index, 4, VertexAttribType.Int, true, offset);
                    break;
                case FormatType.R8G8B8A8_SInt:
                    glInstance.VertexAttribFormat(index, 4, VertexAttribType.Int, false, offset);
                    break;
                case FormatType.B8G8R8A8_UNorm:
                    glInstance.VertexAttribFormat(index, 4, VertexAttribType.UnsignedInt, true, offset);
                    break;
                case FormatType.B8G8R8A8_UNorm_SRGB:
                    throw new NotImplementedException(); //Not sure.
                                                         //glInstance.VertexAttribFormat(index, 4, VertexAttribType.UnsignedInt, true, offset);
                                                         //break;
                case FormatType.R10G10B10A2_UNorm:
                    glInstance.VertexAttribFormat(index, 4, VertexAttribType.UnsignedInt, true, offset);
                    break;
                case FormatType.R10G10B10A2_UInt:
                    glInstance.VertexAttribFormat(index, 4, VertexAttribType.UnsignedInt, false, offset);
                    break;
                case FormatType.R11G11B10_Float:
                    throw new NotImplementedException(); //Not sure.
                                                         //glInstance.VertexAttribFormat(index, 3, VertexAttribType.Float, false, offset);
                                                         //break;

                case FormatType.R32_Float:
                    glInstance.VertexAttribFormat(index, 1, VertexAttribType.Float, false, offset);
                    break;
                case FormatType.R32G32_Float:
                    glInstance.VertexAttribFormat(index, 2, VertexAttribType.Float, false, offset);
                    break;
                case FormatType.R32G32B32_Float:
                    glInstance.VertexAttribFormat(index, 3, VertexAttribType.Float, false, offset);
                    break;
                case FormatType.R32G32B32A32_Float:
                    glInstance.VertexAttribFormat(index, 4, VertexAttribType.Float, false, offset);
                    break;
            }
        }

        private static uint CreateProgram(GL glInstance, GLShader glShader)
        {
            uint program = glInstance.CreateProgram();
            glInstance.AttachShader(program, glShader.Shader);
            glInstance.ProgramParameter(program, ProgramParameterPName.Separable, (int)GLEnum.True);

            glInstance.LinkProgram(program);
            glInstance.GetProgram(program, ProgramPropertyARB.LinkStatus, out int status);
            if (status != (int)GLEnum.True)
                throw new Exception($"Failed to link program: {glInstance.GetProgramInfoLog(program)}");

            glInstance.DetachShader(program, glShader.Shader);

            return program;
        }
    }
}
