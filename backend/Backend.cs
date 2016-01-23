using Compiler.intermediate;
using Compiler.message;

namespace Compiler.backend
{
    public abstract class Backend : IMessageProducer
    {
        public ISymbolTable SymbolTable { get; protected set; }
        public ICode Code { get; protected set; }
        public event MessageEventHandler MessageHandler;

        public abstract void Process(ICode code, ISymbolTable symbol);

        public void SendMessage(Message message)
        {
            MessageHandler?.Invoke(this, message);
        }
    }
}