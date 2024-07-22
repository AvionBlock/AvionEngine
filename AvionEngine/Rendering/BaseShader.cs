using AvionEngine.Interfaces;
using System;

namespace AvionEngine.Rendering
{
    public abstract class BaseShader : IRenderable
    {
        protected IShader? NativeShader { get; set; } //We can swap out native shaders if we need to. Give the option to the user to set or ignore setting the NativeShader.

        /// <summary>
        /// Called when a native shader loads the base shader.
        /// </summary>
        /// <param name="nativeShader">The native shader that loaded the base shader.</param>
        /// <param name="vertexCode">The vertex shader code that the native shader uploads to the GPU.</param>
        /// <param name="fragmentCode">The fragment shader code that the native shader uploads to the GPU.</param>
        public abstract void Load(IShader nativeShader, out string vertexCode, out string fragmentCode);

        public virtual void Reload()
        {
            throw new NotImplementedException();
        }

        public virtual void Render(double delta)
        {
            NativeShader?.Render(delta);
        }

        //All other functions could be imported via extensions, We only care about these functions for the BaseShader.
    }
}