using System;

namespace Compiler.message
{
    public class Message : EventArgs
    {
        public MessageType Type { get; }
        public object Body { get; }

        public Message(MessageType type, object body)
        {
            Type = type;
            Body = body;
        }
    }
}