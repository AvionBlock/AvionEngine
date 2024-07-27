using AvionEngine.Interfaces;
using System;

namespace AvionEngine.Rendering
{
    public class BaseShader : IRenderable
    {
        private IShader nativeShader;
        public virtual IShader NativeShader
        { 
            get => nativeShader;
            set => nativeShader = value;
        } //We can swap out native shaders if we need to. Give the option to the user to set or ignore setting the NativeShader.

        public BaseShader(IRenderer renderer, string vertex, string fragment)
        {
            nativeShader = renderer.CreateShader(vertex, fragment);
        }

        public BaseShader(IShader nativeShader)
        {
            this.nativeShader = nativeShader;
        }

        public virtual void Reload()
        {
            throw new NotImplementedException();
        }

        public virtual void Render(double delta)
        {
            NativeShader.Render(delta);
        }
    }
}