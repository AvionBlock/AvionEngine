using AvionEngine.Interfaces;

namespace AvionEngine.Commands
{
    public struct SetViewportCommand : ICommandAction
    {
        public readonly int X;
        public readonly int Y;
        public readonly uint Width;
        public readonly uint Height;

        public CommandType CommmandType => CommandType.SetViewport;

        public SetViewportCommand(int x, int y, uint width, uint height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
