namespace Compiler.message
{
    public class Message
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