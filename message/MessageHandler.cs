using System;
using System.Collections.Generic;

namespace Compiler.message
{
    public class MessageHandler
    {
        private Message _message;
        private readonly List<IMessageListener> _listeners;

        public MessageHandler()
        {
            _listeners = new List<IMessageListener>();
        }

        public void AddListener(IMessageListener listener)
        {
            _listeners.Add(listener);
        }

        public void RemoveListener(IMessageListener listener)
        {
            _listeners.Remove(listener);
        }

        public void SendMessage(Message message)
        {
            _message = message;
            NotifyListeners();
        }

        private void NotifyListeners()
        {
            foreach (var listener in _listeners)
            {
                listener.MessageReceived(_message);
            }
        }
    }
}