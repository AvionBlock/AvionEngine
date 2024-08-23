using AvionEngine.Interfaces;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using System.Drawing;
using Silk.NET.Maths;
using AvionEngine.Enums;
using AvionEngine.Structures;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace AvionEngine.OpenGL
{
    public class Renderer : IRenderer
    {
        private ConcurrentQueue<Action> Executions;

        public readonly GL glContext;
        public IWindow Window { get; }

        public Renderer(IWindow window)
        {
            Window = window;
            glContext = window.CreateOpenGL();
            Executions = new ConcurrentQueue<Action>();
        }

        public IShader CreateShader(string vertex, string fragment)
        {
            return new Rendering.Shader(this, vertex, fragment);
        }

        public IMesh CreateMesh<TVertex>(TVertex[] vertices, uint[] indices, UsageMode usageMode = UsageMode.Static, DrawMode drawMode = DrawMode.Triangles) where TVertex : unmanaged
        {
            var mesh = new Rendering.Mesh(this, usageMode, drawMode);
            mesh.Update(vertices, indices);
            return mesh;
        }

        public ITexture CreateTexture(
            TextureInfo textureData,
            TextureTargetMode targetMode = TextureTargetMode.Texture2D, 
            TextureFormatMode formatMode = TextureFormatMode.RGB,
            WrapMode wrapModeS = WrapMode.Repeat,
            WrapMode wrapModeT = WrapMode.Repeat,
            WrapMode wrapModeR = WrapMode.Repeat,
            MinFilterMode minFilterMode = MinFilterMode.Linear,
            MagFilterMode magFilterMode = MagFilterMode.Linear)
        {
            return new Rendering.Texture(this, textureData, targetMode, formatMode, wrapModeS, wrapModeT, wrapModeR, minFilterMode, magFilterMode);
        }

        public ITexture CreateTexture(
            TextureInfo[] textureData,
            TextureTargetMode targetMode = TextureTargetMode.Texture2D,
            TextureFormatMode formatMode = TextureFormatMode.RGB,
            WrapMode wrapModeS = WrapMode.Repeat,
            WrapMode wrapModeT = WrapMode.Repeat,
            WrapMode wrapModeR = WrapMode.Repeat,
            MinFilterMode minFilterMode = MinFilterMode.Linear,
            MagFilterMode magFilterMode = MagFilterMode.Linear)
        {
            return new Rendering.Texture(this, textureData, targetMode, formatMode, wrapModeS, wrapModeT, wrapModeR, minFilterMode, magFilterMode);
        }

        public void Execute(Action action)
        {
            Executions.Enqueue(action);
        }

        public void ExecuteQueue()
        {
            while (Executions.Any())
            {
                if (Executions.TryDequeue(out var action))
                {
                    action.Invoke();
                }
            }
        }

        public void Resize(Vector2D<int> newSize)
        {
            glContext.Viewport(newSize);
        }

        public void SetClearColor(Color color)
        {
            glContext.ClearColor(color);
        }

        public void Clear()
        {
            glContext.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
    }
}
