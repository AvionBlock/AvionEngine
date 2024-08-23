using AvionEngine.Interfaces;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System;

namespace AvionEngine.OpenGL.Rendering
{
    public class Shader : IShader
    {
        private readonly Renderer renderer;
        private uint program;

        public IRenderer Renderer { get => renderer; }
        public bool IsDisposed { get; private set; }

        public Shader(Renderer renderer, string vertex, string fragment)
        {
            this.renderer = renderer;
            Load(vertex, fragment);
        }

        public void Render(double delta)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            renderer.glContext.UseProgram(program);
        }

        public void Update(string vertexCode, string fragmentCode)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            renderer.glContext.DeleteProgram(program);
            Load(vertexCode, fragmentCode);
        }

        public void SetBool(string name, bool value)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            renderer.glContext.Uniform1(renderer.glContext.GetUniformLocation(program, name), value ? 1 : 0);
        }

        public void SetInt(string name, int value)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            renderer.glContext.Uniform1(renderer.glContext.GetUniformLocation(program, name), value);
        }

        public void SetUInt(string name, uint value)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            renderer.glContext.Uniform1(renderer.glContext.GetUniformLocation(program, name), value);
        }

        public void SetFloat(string name, float value)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            renderer.glContext.Uniform1(renderer.glContext.GetUniformLocation(program, name), value);
        }

        public unsafe void SetUniform2(string name, Matrix2X2<float> value)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            renderer.glContext.UniformMatrix2(renderer.glContext.GetUniformLocation(program, name), 1, true, (float*)&value);
        }

        public unsafe void SetUniform3(string name, Matrix3X3<float> value)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            renderer.glContext.UniformMatrix3(renderer.glContext.GetUniformLocation(program, name), 1, true, (float*)&value);
        }

        public unsafe void SetUniform4(string name, Matrix4X4<float> value)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            renderer.glContext.UniformMatrix4(renderer.glContext.GetUniformLocation(program, name), 1, true, (float*)&value);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if(disposing)
            {
                renderer.glContext.DeleteProgram(program);
            }

            IsDisposed = true;
        }

        ~Shader()
        {
            Dispose(false);
        }

        public static void Compile(GL glInstance, uint shader)
        {
            glInstance.CompileShader(shader);
            glInstance.GetShader(shader, ShaderParameterName.CompileStatus, out var status);
            if (status != (int)GLEnum.True)
                throw new Exception("Shader failed to compile: " + glInstance.GetShaderInfoLog(shader));
        }

        private void Load(string vertexCode, string fragmentCode)
        {
            //Create Vertex Shader
            var vertex = renderer.glContext.CreateShader(ShaderType.VertexShader);
            renderer.glContext.ShaderSource(vertex, vertexCode);

            //Create Fragment Shader
            var fragment = renderer.glContext.CreateShader(ShaderType.FragmentShader);
            renderer.glContext.ShaderSource(fragment, fragmentCode);
            try
            {
                //Compile and cleanup on error before rethrowing.
                Compile(renderer.glContext, vertex);
                Compile(renderer.glContext, fragment);
            }
            catch (Exception)
            {
                renderer.glContext.DeleteShader(vertex); //Cleanup
                renderer.glContext.DeleteShader(fragment);
                throw;
            }

            program = renderer.glContext.CreateProgram(); //Create the shader itself.

            //attach and link the vertex and fragment shaders to the program.
            renderer.glContext.AttachShader(program, vertex);
            renderer.glContext.AttachShader(program, fragment);
            renderer.glContext.LinkProgram(program);

            //Error Handling.
            renderer.glContext.GetProgram(program, ProgramPropertyARB.LinkStatus, out int lStatus);
            if (lStatus != (int)GLEnum.True)
                throw new Exception("Program failed to link: " + renderer.glContext.GetProgramInfoLog(program));

            //Delete Vertex and Fragment since they are now linked.
            renderer.glContext.DeleteShader(vertex);
            renderer.glContext.DeleteShader(fragment);
        }
    }
}