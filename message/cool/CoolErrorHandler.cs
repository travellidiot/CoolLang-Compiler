using System;
using Compiler.frontend;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate.cool.symtab;

namespace Compiler.message.cool
{
    public class CoolErrorHandler
    {
        private static int MaxErrors => 25;
        //public static CoolErrorHandler Instance { get; } = new CoolErrorHandler();
        public static int ErrorCount { get; private set; }

        private CoolErrorHandler() { }

        public static void FlagSyntaxError(Token token, CoolErrorCode errorCode, IMessageProducer producer)
        {
            producer.SendMessage(new Message(MessageType.SyntaxError,
                                            new object[]
                                            {
                                                token.LineNumber,
                                                token.Position,
                                                token.Text,
                                                token.Type,
                                                errorCode.ToString()
                                            }));
            if (++ErrorCount > MaxErrors)
            {
                AbortTranslation(CoolErrorCode.TooManyErrors, producer);
            }
        }

        public static void FlagSemanticsError(Token token, CoolErrorCode errorCode, IMessageProducer producer)
        {
            producer.SendMessage(new Message(MessageType.SemanticsError,
                                            new object[]
                                            {
                                                token.LineNumber,
                                                token.Position,
                                                token.Text,
                                                token.Type,
                                                errorCode.ToString()
                                            }));
            if (++ErrorCount > MaxErrors)
            {
                AbortTranslation(CoolErrorCode.TooManyErrors, producer);
            }
        }

        public static void AssertTypeDefined(ClassSymbol clsSymbol, CoolWordToken clsName, IMessageProducer producer)
        {
            if (clsSymbol == null)
            {
                FlagSemanticsError(
                    clsName,
                    CoolErrorCode.UndefinedType,
                    producer);
            }
        }
        public static void Flag(IMessageProducer producer, Message message)
        {
            producer.SendMessage(message);
            if (++ErrorCount > MaxErrors)
            {
                AbortTranslation(CoolErrorCode.TooManyErrors, producer);
            }
        }

        public static void AbortTranslation(CoolErrorCode errorCode, IMessageProducer producer)
        {
            var fatalText = "Fatal Error: " + errorCode.ToString();
            throw new FatalErrorException(fatalText);
        }

        public class FatalErrorException : Exception
        {
            private readonly string _errMsg;

            public FatalErrorException(string errMsg)
            {
                _errMsg = errMsg;
            }

            public override string ToString()
            {
                return _errMsg;
            }
        }
    }
}
