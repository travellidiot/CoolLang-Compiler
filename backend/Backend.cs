using Compiler.intermediate;
using Compiler.message;

namespace Compiler.backend
{
    public abstract class Backend : IMessageProducer
    {
        public static MessageHandler MessageHandler { get; } = new MessageHandler();
        public ISymbolTable SymbolTable { get; protected set; }
        public ICode Code { get; protected set; }

        public abstract void Process(ICode code, ISymbolTable symbol);

        public void AddMessageListener(IMessageListener listener)
        {
            MessageHandler.AddListener(listener);
        }

        public void RemoveMessageListener(IMessageListener listener)
        {
            MessageHandler.RemoveListener(listener);
        }

        public void SendMessage(Message message)
        {
            MessageHandler.SendMessage(message);
        }
    }
}