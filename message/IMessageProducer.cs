using System;
using System.Collections.Generic;

namespace Compiler.message
{
    //public delegate void MessageEventHandler(object sender, Message e);
    public interface IMessageProducer
    {
        void AddListener(IMessageListener listener);
        void RemoveListener(IMessageListener listener);
        void SendMessage(Message message);
    }
}