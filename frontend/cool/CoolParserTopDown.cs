using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.frontend;
using Compiler.message;
using Compiler.utils;

namespace Compiler.frontend.cool
{
    class CoolParserTopDown : Parser
    {
        protected static CoolErrorHandler ErrorHandler = new CoolErrorHandler();

        public CoolParserTopDown(Scanner scanner) : base(scanner)
        {
        }

        public override void Parse()
        {
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
                        SendMessage(new Message(MessageType.Token,
                            new[]
                            {
                                token.LineNumber,
                                token.Position,
                                tokenType,
                                token.Text,
                                token.Value
                            }));

                        LoggerUtil.LogToken(token);
                    }
                    else
                    {
                        ErrorHandler.Flag(token, (CoolErrorCode) token.Value, this);
                    }
                }

                sw.Start();
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
    }
}
