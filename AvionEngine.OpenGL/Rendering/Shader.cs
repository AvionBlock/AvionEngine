using AvionEngine.Interfaces;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System;

namespace AvionEngine.OpenGL.Rendering
{
    public class Shader : IShader
    {
        private readonly GL glInstance;
        private uint id;

        public bool IsDisposed { get; private set; }

        public Shader(GL glInstance, string vertex, string fragment)
        {
            this.glInstance = glInstance;
            Load(vertex, fragment);
        }

        public void Render(double delta)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            glInstance.UseProgram(id);
        }

        public void Update(string vertexCode, string fragmentCode)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            glInstance.DeleteProgram(id);
            Load(vertexCode, fragmentCode);
        }

        public void SetBool(string name, bool value)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            glInstance.Uniform1(glInstance.GetUniformLocation(id, name), value ? 1 : 0);
        }

        public void SetInt(string name, int value)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            glInstance.Uniform1(glInstance.GetUniformLocation(id, name), value);
        }

        public void SetUInt(string name, uint value)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            glInstance.Uniform1(glInstance.GetUniformLocation(id, name), value);
        }

        public void SetFloat(string name, float value)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            glInstance.Uniform1(glInstance.GetUniformLocation(id, name), value);
        }

        public unsafe void SetUniform2(string name, Matrix2X2<float> value)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            glInstance.UniformMatrix2(glInstance.GetUniformLocation(id, name), 1, true, (float*)&value);
        }

        public unsafe void SetUniform3(string name, Matrix3X3<float> value)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            glInstance.UniformMatrix3(glInstance.GetUniformLocation(id, name), 1, true, (float*)&value);
        }

        public unsafe void SetUniform4(string name, Matrix4X4<float> value)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Shader));

            glInstance.UniformMatrix4(glInstance.GetUniformLocation(id, name), 1, true, (float*)&value);
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
                glInstance.DeleteProgram(id);
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
            var vertex = glInstance.CreateShader(ShaderType.VertexShader);
            glInstance.ShaderSource(vertex, vertexCode);

            //Create Fragment Shader
            var fragment = glInstance.CreateShader(ShaderType.FragmentShader);
            glInstance.ShaderSource(fragment, fragmentCode);
            try
            {
                //Compile and cleanup on error before rethrowing.
                Compile(glInstance, vertex);
                Compile(glInstance, fragment);
            }
            catch (Exception)
            {
                glInstance.DeleteShader(vertex); //Cleanup
                glInstance.DeleteShader(fragment);
                throw;
            }

            id = glInstance.CreateProgram(); //Create the shader itself.

            //attach and link the vertex and fragment shaders to the program.
            glInstance.AttachShader(id, vertex);
            glInstance.AttachShader(id, fragment);
            glInstance.LinkProgram(id);

            //Error Handling.
            glInstance.GetProgram(id, ProgramPropertyARB.LinkStatus, out int lStatus);
            if (lStatus != (int)GLEnum.True)
                throw new Exception("Program failed to link: " + glInstance.GetProgramInfoLog(id));

            //Delete Vertex and Fragment since they are now linked.
            glInstance.DeleteShader(vertex);
            glInstance.DeleteShader(fragment);
        }
    }
}