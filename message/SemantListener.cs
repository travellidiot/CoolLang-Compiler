using System;
using Compiler.frontend.cool;


namespace Compiler.message
{
    class SemantListener : IMessageListener
    {
        private static string SemanticsErrorFormat =>
            "\nline {0}, col {1}: \"{2}\":{3}, {4}.\n";
        public SemantListener(IMessageProducer producer)
        {
            producer.AddListener(this);
        }

        public void MessageReceived(Message message)
        {
            var type = message.Type;
            var bodies = (object[])message.Body;

            switch (type)
            {
                case MessageType.SemanticsError:
                    var body = (object[])message.Body;
                    var lineNumber = (int)body[0];
                    var position = (int)body[1];
                    var text = (string)body[2];
                    var tokenType = (CoolTokenType)body[3];
                    var errorCode = (string)body[4];
                    Console.WriteLine(SemanticsErrorFormat, lineNumber, position, text, tokenType.CoolType, errorCode);
                    break;
            }
        }
    }
}