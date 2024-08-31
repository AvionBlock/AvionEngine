using AvionEngine.Interfaces;

namespace AvionEngine.Commands
{
    public struct RenderCommand : ICommandAction
    {
        public CommandType CommmandType => CommandType.Render;
    }
}
