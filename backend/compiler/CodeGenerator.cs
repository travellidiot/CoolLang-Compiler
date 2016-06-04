using System;
using System.Diagnostics;

using Compiler.intermediate;
using Compiler.message;

namespace Compiler.backend.compiler
{
    internal class CodeGenerator : Backend
    {
        public override void Process(IAst code, ISymTabStack symbol)
        {
            var s = new Stopwatch();
            s.Start();
            Console.WriteLine("Generating code...");
            s.Stop();
            var elapsedTime = s.ElapsedMilliseconds;
            Console.WriteLine("Esplased time: {0}", s.ElapsedMilliseconds);

            var instructionCount = 0;
            SendMessage(new Message(MessageType.CompilerSummary,
                                    new object[] {instructionCount, elapsedTime}));
        }
    }
}