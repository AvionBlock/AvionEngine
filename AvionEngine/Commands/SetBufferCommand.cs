using AvionEngine.Graphics;
using AvionEngine.Interfaces;

namespace AvionEngine.Commands
{
    public unsafe struct SetBufferCommand : ICommandAction
    {
        public readonly AVBuffer Buffer;
        public readonly uint SizeInBytes;
        public readonly void* Data;

        public CommandType CommmandType => CommandType.SetBuffer;

        public SetBufferCommand(AVBuffer buffer, uint sizeInBytes, void* data)
        {
            Buffer = buffer;
            SizeInBytes = sizeInBytes;
            Data = data;
        }
    }
}
