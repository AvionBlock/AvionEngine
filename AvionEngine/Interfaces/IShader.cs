﻿using System.Numerics;

namespace AvionEngine.Interfaces
{
    public interface IShader
    {
        void Reload(string vertexCode, string fragmentCode);

        void SetBool(string name, bool value);

        void SetInt(string name, int value);

        void SetUInt(string name, uint value);

        void SetFloat(string name, float value);

        void SetUniform4(string name, Matrix4x4 value);

        void Bind();
    }
}
