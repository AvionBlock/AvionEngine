using Arch.Core;
using AvionEngine.Enums;
using AvionEngine.Interfaces;
using AvionEngine.Rendering;
using AvionEngine.Structures;

namespace AvionEngine
{
    public class AvionEngine : IEngine
    {
        public IRenderer Renderer { get; private set; } //We can swap out rendering engines.
        public World World { get; private set; } //Swappable Worlds

        public AvionEngine(IRenderer renderer)
        {
            Renderer = renderer; //Set the renderer the user wants first.
            World = World.Create();

            renderer.Window.Resize += Resize;
            renderer.Window.Update += Update;
        }

        public void SetRenderer(IRenderer renderer)
        {
            Renderer.Window.Update -= Update;

            Renderer = renderer;

            Renderer.Window.Update += Update;
            //NOT FINISHED!
        }

        public BaseShader CreateShader(string vertex, string fragment)
        {
            return new BaseShader(Renderer.CreateShader(vertex, fragment));
        }

        public BaseMesh CreateMesh<TVertex>(TVertex[] vertices, uint[] indices, UsageMode usageMode = UsageMode.Static, DrawMode drawMode = DrawMode.Triangles) where TVertex : unmanaged
        {
            return new BaseMesh(Renderer.CreateMesh(vertices, indices, usageMode, drawMode));
        }

        public BaseTexture CreateTexture(
            TextureInfo textureData,
            TextureTargetMode targetMode = TextureTargetMode.Texture2D,
            TextureFormatMode formatMode = TextureFormatMode.RGB,
            WrapMode wrapModeS = WrapMode.Repeat,
            WrapMode wrapModeT = WrapMode.Repeat,
            WrapMode wrapModeR = WrapMode.Repeat,
            MinFilterMode minFilterMode = MinFilterMode.Linear,
            MagFilterMode magFilterMode = MagFilterMode.Linear)
        {
            return new BaseTexture(Renderer.CreateTexture(textureData, targetMode, formatMode, wrapModeS, wrapModeT, wrapModeR, minFilterMode, magFilterMode));
        }

        public BaseTexture CreateTexture(
            TextureInfo[] textureData,
            TextureTargetMode targetMode = TextureTargetMode.Texture2D,
            TextureFormatMode formatMode = TextureFormatMode.RGB,
            WrapMode wrapModeS = WrapMode.Repeat,
            WrapMode wrapModeT = WrapMode.Repeat,
            WrapMode wrapModeR = WrapMode.Repeat,
            MinFilterMode minFilterMode = MinFilterMode.Linear,
            MagFilterMode magFilterMode = MagFilterMode.Linear)
        {
            return new BaseTexture(Renderer.CreateTexture(textureData, targetMode, formatMode, wrapModeS, wrapModeT, wrapModeR, minFilterMode, magFilterMode));
        }

        private void Resize(Silk.NET.Maths.Vector2D<int> newSize)
        {
            Renderer.Resize(newSize);
        }

        private void Update(double delta)
        {
            Renderer.Clear();
        }
    }
}
