﻿using AvionEngine.Interfaces;
using System;

namespace AvionEngine.Rendering
{
    public abstract class BaseShader : IRenderable
    {
        protected IShader? NativeShader { get; set; } //We can swap out native shaders if we need to.

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

        public virtual void Reload(string vertexCode, string fragmentCode)
        {
            NativeShader?.Reload(vertexCode, fragmentCode);
        }

        public virtual void Render()
        {
            NativeShader?.Render();
        }
    }
}