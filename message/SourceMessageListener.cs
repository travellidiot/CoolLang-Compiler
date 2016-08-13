using System;

namespace Compiler.message
{

    class SourceMessageListener : IMessageListener
    {
        static string SourceLineFormat => "%03d %s";
        public SourceMessageListener(IMessageProducer producer)
        {
            producer.AddListener(this);
        }

        public void MessageReceived(Message message)
        {
            var type = message.Type;
            var bodies = (object[])message.Body;

            switch (type)
            {
                case MessageType.SourceLine:
                    {
                        var lineNumber = (int)bodies[0];
                        var lineText = (string)bodies[1];
                        Console.WriteLine(SourceLineFormat, lineNumber, lineText);
                        break;
                    }
            }
        }
    }
}