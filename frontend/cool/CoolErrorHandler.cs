﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.message;

namespace Compiler.frontend.cool
{
    public class CoolErrorHandler
    {
        private static int MaxErrors => 25;
        public static int ErrorCount { get; private set; } = 0;

        public void Flag(Token token, CoolErrorCode errorCode, Parser parser)
        {
            parser.SendMessage(new Message(MessageType.SyntaxError,
                                            new object[]
                                            {
                                                token.LineNumber,
                                                token.Position,
                                                token.Text,
                                                errorCode.ToString()
                                            }));
            if (++ErrorCount > MaxErrors)
            {
                AbortTranslation(CoolErrorCode.TooManyErrors, parser);
            }
        }

        public void AbortTranslation(CoolErrorCode errorCode, Parser parser)
        {
            string fatalText = "Fatal Error: " + errorCode.ToString();
            parser.SendMessage(new Message(MessageType.SyntaxError,
                new object[] {0, 0, "", fatalText}));

            throw new Exception(fatalText);
        }
    }
}