using AvionEngine.Interfaces;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using System.Drawing;
using Silk.NET.Maths;
using AvionEngine.Enums;
using AvionEngine.Structures;

namespace AvionEngine.OpenGL
{
    public class Renderer : IRenderer
    {
        public IWindow Window { get; }
        private readonly GL glInstance;

        public Renderer(IWindow window)
        {
            Window = window;
            glInstance = window.CreateOpenGL();
        }

        public IShader CreateShader(string vertex, string fragment)
        {
            return new Rendering.Shader(glInstance, vertex, fragment);
        }

        public IMesh CreateMesh<TVertex>(TVertex[] vertices, uint[] indices, UsageMode usageMode = UsageMode.Static, DrawMode drawMode = DrawMode.Triangles) where TVertex : unmanaged
        {
            var mesh = new Rendering.Mesh(glInstance, usageMode);
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
            return new Rendering.Texture(glInstance, textureData, targetMode, formatMode, wrapModeS, wrapModeT, wrapModeR, minFilterMode, magFilterMode);
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
            return new Rendering.Texture(glInstance, textureData, targetMode, formatMode, wrapModeS, wrapModeT, wrapModeR, minFilterMode, magFilterMode);
        }

        public void Resize(Vector2D<int> newSize)
        {
            glInstance.Viewport(newSize);
        }

        public void SetClearColor(Color color)
        {
            glInstance.ClearColor(color);
        }

        public void Clear()
        {
            glInstance.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
    }
}
