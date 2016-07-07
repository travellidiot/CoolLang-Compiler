using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Compiler.frontend.cool.parsers;
using Compiler.intermediate;
using Compiler.intermediate.cool.ast;
using Compiler.intermediate.cool.symtab;
using Compiler.message;

namespace Compiler.frontend.cool
{
    public class CoolTdParser : Parser
    {
        protected static CoolErrorHandler ErrorHandler = CoolErrorHandler.Instance;

        public CoolTdParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolTdParser(CoolTdParser parent) : base(parent.Scanner)
        {
            Listeners = parent.Listeners;
        }

        public override IAstNode Parse()
        {
            //var logger = new LoggerUtil(new StreamWriter(Console.OpenStandardOutput()));
            var globalScope = GlobalSymbolScope.Instance;

            ScopeStack.Push(globalScope);

            try
            {
                var sw = new Stopwatch();
                sw.Start();

                //NextToken();
                var parser = new CoolProgramParser(this);
                AstRoot = new CoolAst { Root = parser.Parse() };

                sw.Stop();
                var elapsedTime = sw.ElapsedMilliseconds;
                SendMessage(new Message(MessageType.ParserSummary,
                    new object[] {CurrentToken().LineNumber, GetErrorCount(), elapsedTime}));
            }
            catch (IOException)
            {
                ErrorHandler.AbortTranslation(CoolErrorCode.IOError, this);
            }

            return AstRoot.Root;
        }

        public new Token CurrentToken()
        {
            return base.CurrentToken();
        }
        public new Token NextToken()
        {
            return base.NextToken();
        }

        public override int GetErrorCount()
        {
            return CoolErrorHandler.ErrorCount;
        }

        public Token Synchronize(ISet<ITokenType> syncSet)
        {
            var token = CurrentToken();
            while (Equals(token.Type, CoolTokenType.Comment))
                token = NextToken();

            if (syncSet.Contains(token.Type)) return token;

            ErrorHandler.Flag(token, CoolErrorCode.UnExpectedToken, this);
            do
            {
                token = NextToken();
            } while (token.GetType() != typeof (EofToken) && !syncSet.Contains(token.Type));

            if (token.GetType() == typeof(EofToken))
            {
                ErrorHandler.AbortTranslation(CoolErrorCode.UnExpectedEof, this);
            }

            return token;
        }
    }
}
