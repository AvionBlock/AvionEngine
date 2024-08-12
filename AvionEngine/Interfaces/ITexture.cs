using System;
using AvionEngine.Enums;

namespace AvionEngine.Interfaces
{
    public interface ITexture : IVisual, IDisposable
    {
        void Assign(int unit = 0);
    }
}
