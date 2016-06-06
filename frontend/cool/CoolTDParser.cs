using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Compiler.frontend;
using Compiler.intermediate;
using Compiler.message;
using Compiler.utils;

namespace Compiler.frontend.cool
{
    public class CoolTDParser : Parser
    {
        protected static CoolErrorHandler ErrorHandler = new CoolErrorHandler();

        public CoolTDParser(Scanner scanner) : base(scanner)
        {
        }

        public override void Parse()
        {
            LoggerUtil logger = new LoggerUtil(new StreamWriter(Console.OpenStandardOutput()));

            try
            {
                var sw = new Stopwatch();
                Token token;

                sw.Start();
                while (!((token = NextToken()).GetType() == typeof (EofToken)))
                {
                    CoolTokenType tokenType = token.Type as CoolTokenType;
                    if (tokenType?.CoolType != TokenType.Error)
                    {
                        
                    }
                    else
                    {
                        ErrorHandler.Flag(token, (CoolErrorCode) token.Value, this);
                    }
                }

                sw.Stop();
                var elapsedTime = sw.ElapsedMilliseconds;
                SendMessage(new Message(MessageType.ParserSummary,
                    new object[] {token.LineNumber, GetErrorCount(), elapsedTime}));
            }
            catch (System.IO.IOException ex)
            {
                ErrorHandler.AbortTranslation(CoolErrorCode.IOError, this);
            }
        }

        public override int GetErrorCount()
        {
            return CoolErrorHandler.ErrorCount;
        }

        public Token Synchronize(ISet<TokenType> syncSet)
        {
            Token token = CurrentToken();
            CoolTokenType t = token.Type as CoolTokenType;
            if (syncSet.Contains(t.CoolType)) return token;

            ErrorHandler.Flag(token, CoolErrorCode.UnExpectedToken, this);
            do
            {
                token = NextToken();
                t = token.Type as CoolTokenType;
            } while (!(token.GetType() == typeof (EofToken)) && !syncSet.Contains(t.CoolType));

            return token;
        }
    }
}
