using System;

namespace Compiler.message
{
    public delegate void MessageEventHandler(object sender, Message e);
    public interface IMessageProducer
    {
        event EventHandler<Message> MessageHandler;
        void SendMessage(Message message);
    }
}