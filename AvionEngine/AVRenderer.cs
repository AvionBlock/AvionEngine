using AvionEngine.Interfaces;
using Silk.NET.Windowing;
using System;
using System.Collections.Concurrent;

namespace AvionEngine
{
    public abstract class AVRenderer : IDisposable
    {
        public readonly ConcurrentQueue<ICommandAction> Commands = new ConcurrentQueue<ICommandAction>();
        public readonly IWindow Window;

        public bool IsDisposed { get; protected set; }

        public AVRenderer(IWindow window)
        {
            Window = window;
        }

        public virtual void Execute(in ICommandAction commandAction)
        {
            Commands.Enqueue(commandAction);
        }

        public abstract void ExecuteCommands();

        public abstract void Dispose();
    }
}
