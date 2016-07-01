using System;
using System.Diagnostics;
using Compiler.intermediate;
using Compiler.message;

namespace Compiler.backend.interpreter
{
    internal class Executor : Backend
    {
        public override void Process(IAst code, IScopeStack symbol)
        {
            var s = new Stopwatch();
            s.Start();
            Console.WriteLine("Generating code...");
            s.Stop();
            var elapsedTime = s.ElapsedMilliseconds;
            Console.WriteLine("Esplased time: {0}", s.ElapsedMilliseconds);

            var instructionCount = 0;
            var runtimeErrors = 0;
            SendMessage(new Message(MessageType.InterpreterSummary,
                                    new object[] { instructionCount, runtimeErrors, elapsedTime }));
        }
    }
}