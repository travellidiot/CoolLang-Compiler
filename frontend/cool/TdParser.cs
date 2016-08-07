using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Compiler.frontend.cool.parsers;
using Compiler.intermediate;
using Compiler.intermediate.cool.ast;
using Compiler.intermediate.cool.symtab;
using Compiler.message;
using Compiler.message.cool;
using static Compiler.message.cool.ErrorHandler;

namespace Compiler.frontend.cool
{
    public class TdParser : Parser
    {        
        public TdParser(frontend.Scanner scanner) : base(scanner)
        {
        }

        public TdParser(TdParser parent) : base(parent.Scanner)
        {
            Listeners = parent.Listeners;
        }

        public override IAstNode Parse()
        {
            try
            {
                var sw = new Stopwatch();
                sw.Start();

                //NextToken();
                var parser = new ProgramParser(this);
                AstRoot = new Ast { Root = parser.Parse() };

                sw.Stop();
                var elapsedTime = sw.ElapsedMilliseconds;
                SendMessage(new Message(MessageType.ParserSummary,
                    new object[] {CurrentToken().LineNumber, GetErrorCount(), elapsedTime}));
            }
            catch (IOException)
            {
                AbortTranslation(ErrorCode.IOError, this);
            }

            return AstRoot.Root;
        }

        public new frontend.Token CurrentToken()
        {
            return base.CurrentToken();
        }
        public new frontend.Token NextToken()
        {
            return base.NextToken();
        }

        public override int GetErrorCount()
        {
            return ErrorHandler.ErrorCount;
        }

        public frontend.Token Synchronize(ISet<ITokenType> syncSet)
        {
            var token = CurrentToken();
            while (Equals(token.Type, CoolTokenType.Comment))
                token = NextToken();

            if (syncSet.Contains(token.Type)) return token;

            FlagSyntaxError(token, ErrorCode.UnExpectedToken, this);
            do
            {
                token = NextToken();
            } while (token.GetType() != typeof (EofToken) && !syncSet.Contains(token.Type));

            if (token.GetType() == typeof(EofToken))
            {
                AbortTranslation(ErrorCode.UnExpectedEof, this);
            }

            return token;
        }
    }
}
