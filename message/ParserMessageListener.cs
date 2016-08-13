using System;
using Compiler.frontend.cool;

namespace Compiler.message
{
    class ParserMessageListener : IMessageListener
    {
        private static string ParserSummaryFormat =>
             "\n{0} source lines." +
             "\n{1} syntax errors." +
             "\n{2} milliseconds total parsing time.\n";

        private static string SyntaxErrorFormat =>
            "\nline {0}, col {1}: \"{2}\":{3}, {4}.\n";

        public ParserMessageListener(IMessageProducer messageProducer)
        {
            messageProducer.AddListener(this);
        }

        public void MessageReceived(Message message)
        {
            var type = message.Type;

            switch (type)
            {
                case MessageType.ParserSummary:
                    var bodies = (object[])message.Body;
                    int statementCount = (int)bodies[0];
                    int syntaxErrors = (int)bodies[1];
                    long elapsedTime = (long)bodies[2];

                    Console.WriteLine(ParserSummaryFormat,
                                      statementCount, syntaxErrors,
                                      elapsedTime);
                    break;

                case MessageType.SyntaxError:
                    var body = (object[])message.Body;
                    var lineNumber = (int)body[0];
                    var position = (int)body[1];
                    var text = (string)body[2];
                    var tokenType = (CoolTokenType)body[3];
                    var errorCode = (string)body[4];
                    Console.WriteLine(SyntaxErrorFormat, lineNumber, position, text, tokenType.CoolType, errorCode);
                    break;
            }
        }
    }
}