namespace Compiler.message
{
    public interface IMessageListener
    {
        void MessageReceived(object sender, Message message);
    }
}