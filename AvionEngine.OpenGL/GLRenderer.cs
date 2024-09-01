using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using System;
using AvionEngine.Commands;
using AvionEngine.OpenGL.Graphics;
using Silk.NET.SDL;

namespace AvionEngine.OpenGL
{
    public class GLRenderer : AVRenderer
    {
        public readonly GL glContext;

        public GLRenderer(IWindow window) : base(window)
        {
            glContext = window.CreateOpenGL();
        }

        public override unsafe void ExecuteCommands()
        {
            foreach (var command in Commands)
            {
                switch(command.CommmandType)
                {
                    case CommandType.Action:
                        var actionCommand = (ActionCommand)command;
                        actionCommand.Action.Invoke();
                        break;
                    case CommandType.BeginRender:
                        break;
                    case CommandType.EndRender:
                        break;
                    case CommandType.UpdateBuffer:
                        var updateBufferCommand = (UpdateBufferCommand)command;
                        var updateBuffer = (GLBuffer)updateBufferCommand.Buffer;

                        updateBuffer.Bind();
                        glContext.BufferSubData(GLBuffer.GetBufferTargetARB(updateBuffer.BufferType), (IntPtr)updateBufferCommand.Offset, updateBufferCommand.Size, updateBufferCommand.Data);

                        //Unbind to be safe.
                        updateBuffer.Unbind();
                        break;
                    case CommandType.SetBuffer:
                        var setBufferCommand = (SetBufferCommand)command;
                        var setBuffer = (GLBuffer)setBufferCommand.Buffer;

                        setBuffer.Bind();
                        glContext.BufferData(GLBuffer.GetBufferTargetARB(setBuffer.BufferType), setBufferCommand.SizeInBytes, setBufferCommand.Data, GLBuffer.GetBufferUsageARB(setBuffer.BufferUsageMode));

                        //Unbind to be safe.
                        setBuffer.Unbind();
                        break;
                    case CommandType.DeleteBuffer:
                        var deleteBufferCommand = (UpdateBufferCommand)command;
                        var deleteBuffer = (GLBuffer)deleteBufferCommand.Buffer;

                        deleteBuffer.Dispose();
                        break;

                    case CommandType.SetViewport:
                        var setViewportCommand = (SetViewportCommand)command;

                        glContext.Viewport(setViewportCommand.X, setViewportCommand.Y, setViewportCommand.Width, setViewportCommand.Height);
                        break;
                }
            }
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                glContext.Dispose();
            }

            IsDisposed = true;
        }

        ~GLRenderer()
        {
            Dispose(false);
        }
    }
}
