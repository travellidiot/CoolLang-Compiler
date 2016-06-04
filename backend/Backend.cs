using Compiler.intermediate;
using Compiler.message;
using System;

namespace Compiler.backend
{
    public abstract class Backend : IMessageProducer
    {
        public ISymTab SymbolTable { get; protected set; }
        public IAst Code { get; protected set; }
        public event EventHandler<Message> MessageHandler;

        public abstract void Process(IAst code, ISymTabStack symbol);

        public void SendMessage(Message message)
        {
            MessageHandler?.Invoke(this, message);
        }
    }
}