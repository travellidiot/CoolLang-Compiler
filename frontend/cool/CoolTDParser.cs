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

        public CoolTDParser(CoolTDParser parent) : base(parent.Scanner)
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
                
                while ((token = NextToken()).GetType() != typeof (EofToken))
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


        public new CoolToken CurrentToken()
        {
            return (CoolToken) base.CurrentToken();
        }
        public new CoolToken NextToken()
        {
            return (CoolToken)base.NextToken();
        }

        public override int GetErrorCount()
        {
            return CoolErrorHandler.ErrorCount;
        }

        public CoolToken Synchronize(ISet<TokenType> syncSet)
        {
            CoolToken token = CurrentToken();
            if (syncSet.Contains(token.Type.CoolType)) return token;

            ErrorHandler.Flag(token, CoolErrorCode.UnExpectedToken, this);
            do
            {
                token = NextToken();
            } while (token.GetType() != typeof (EofToken) && !syncSet.Contains(token.Type.CoolType));

            if (token.GetType() != typeof(EofToken))
            {
                ErrorHandler.AbortTranslation(CoolErrorCode.UnExpectedEof, this);
            }

            return token;
        }
    }
}
