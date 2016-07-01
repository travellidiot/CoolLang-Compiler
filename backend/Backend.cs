using Compiler.intermediate;
using Compiler.message;
using System;
using System.Collections.Generic;

namespace Compiler.backend
{
    public abstract class Backend : IMessageProducer
    {
        public IScope SymbolTable { get; protected set; }
        public IAst Code { get; protected set; }
        protected List<IMessageListener> Listeners = new List<IMessageListener>(); 

        public abstract void Process(IAst code, IScopeStack symbol);

        public void AddListener(IMessageListener listener)
        {
            Listeners.Add(listener);
        }

        public void RemoveListener(IMessageListener listener)
        {
            Listeners.Remove(listener);
        }

        public void SendMessage(Message message)
        {
            foreach (var listener in Listeners)
            {
                listener.MessageReceived(message);
            }
        }
    }
}