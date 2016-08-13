using System;
using static Compiler.message.MessageType;

namespace Compiler.message
{

    class BackendMessageListener : IMessageListener
    {
        private static string InterpreterSummaryFormat => "\n{0} statements executed." + "\n{1} runtime errors." + "\n{2} seconds total execution time.\n";
        private static string CompilerSummaryFormat => "\n{0} instructions generated." + "\n{1} seconds total code generation time.\n";
        public BackendMessageListener(IMessageProducer producer)
        {
            producer.AddListener(this);
        }

        public void MessageReceived(Message message)
        {
            MessageType type = message.Type;

            switch (type)
            {
                case InterpreterSummary:
                    {
                        var body = (object[])message.Body;
                        int executionCount = (int)body[0];
                        int runtimeErrors = (int)body[1];
                        long elapsedTime = (long)body[2];

                        Console.WriteLine(InterpreterSummaryFormat, executionCount, runtimeErrors, elapsedTime);
                        break;
                    }

                case CompilerSummary:
                    {
                        var body = (object[])message.Body;
                        int instructionCount = (int)body[0];
                        long elapsedTime = (long)body[1];

                        Console.WriteLine(CompilerSummaryFormat, instructionCount, elapsedTime);
                        break;
                    }
            }
        }
    }
}