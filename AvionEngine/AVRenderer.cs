using AvionEngine.Interfaces;
using Silk.NET.Windowing;
using System.Collections.Concurrent;

namespace AvionEngine
{
    public abstract class AVRenderer
    {
        public readonly ConcurrentQueue<ICommandAction> Commands = new ConcurrentQueue<ICommandAction>();
        public readonly IWindow Window;

        public AVRenderer(IWindow window)
        {
            Window = window;
        }

        public virtual void Execute(in ICommandAction commandAction)
        {
            Commands.Enqueue(commandAction);
        }
    }
}
