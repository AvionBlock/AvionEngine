using AvionEngine.Graphics;
using AvionEngine.Interfaces;

namespace AvionEngine.Commands
{
    public unsafe struct UpdateBufferCommand : ICommandAction
    {
        public readonly AVBuffer Buffer;
        public readonly uint Offset;
        public readonly uint Size;
        public readonly void* Data;

        public CommandType CommmandType => CommandType.UpdateBuffer;

        public UpdateBufferCommand(AVBuffer buffer, uint offset, uint size, void* data)
        {
            Buffer = buffer;
            Offset = offset;
            Size = size;
            Data = data;
        }
    }
}
