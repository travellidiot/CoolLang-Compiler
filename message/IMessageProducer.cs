namespace Compiler.message
{
    public delegate void MessageEventHandler(object sender, Message e);
    public interface IMessageProducer
    {
        event MessageEventHandler MessageHandler;
        void SendMessage(Message message);
    }
}