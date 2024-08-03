using Arch.Core;
using AvionEngine.Enums;
using AvionEngine.Interfaces;
using AvionEngine.Rendering;

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

        public BaseTexture CreateTexture2D(uint width, uint height, byte[] data, TextureFormat format = TextureFormat.RGB)
        {
            return new BaseTexture(Renderer.CreateTexture2D(width, height, data, format));
        }

        public BaseMesh<TVertex> CreateMesh<TVertex>(TVertex[] vertices, uint[] indices, DrawMode drawMode = DrawMode.Static) where TVertex : unmanaged
        {
            return new BaseMesh<TVertex>(Renderer.CreateMesh(vertices, indices, drawMode));
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
