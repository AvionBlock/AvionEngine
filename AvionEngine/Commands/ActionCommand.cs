using AvionEngine.Interfaces;
using System;

namespace AvionEngine.Commands
{
    public struct ActionCommand : ICommandAction
    {
        public readonly Action Action;

        public CommandType CommmandType => CommandType.Action;

        public ActionCommand(Action action)
        {
            Action = action;
        }
    }
}
