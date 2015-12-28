using System.Diagnostics;
using Compiler.frontend;
using Compiler.message;

namespace Compiler.frontend.pascal
{
    public class PascalParserTopDown : Parser
    {
        public PascalParserTopDown(Scanner scanner) : base(scanner)
        {
        }

        public override void Parse()
        {
            Token token;
            var s = new Stopwatch();
            s.Start();
            while (!((token = NextToken()).GetType() == typeof (EofToken))) {}
            s.Stop();
            var elapsedTime = s.ElapsedMilliseconds;
            SendMessage(new Message(MessageType.ParserSummary,
                new object[] {token.LineNumber, GetErrorCount(), elapsedTime}));
        }

        public override int GetErrorCount()
        {
            return 0;
        }
    }
}