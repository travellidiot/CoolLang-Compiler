using System;
using Compiler.frontend;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate.cool.symtab;

namespace Compiler.message.cool
{
    public class ErrorHandler
    {
        private static int MaxErrors => 25;
        //public static ErrorHandler Instance { get; } = new ErrorHandler();
        public static int ErrorCount { get; private set; }

        private ErrorHandler() { }

        public static void FlagSyntaxError(Token token, ErrorCode errorCode,
            IMessageProducer producer)
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
                AbortTranslation(ErrorCode.TooManyErrors, producer);
            }
        }

        public static void FlagSemanticsError(Token token, ErrorCode errorCode,
            IMessageProducer producer)
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
                AbortTranslation(ErrorCode.TooManyErrors, producer);
            }
        }

        public static void AssertTypeDefined(ClassSymbol clsSymbol, WordToken clsName,
            IMessageProducer producer)
        {
            if (clsSymbol == null)
            {
                FlagSemanticsError(
                    clsName,
                    ErrorCode.UndefinedType,
                    producer);
            }
        }
        public static void Flag(IMessageProducer producer, Message message)
        {
            producer.SendMessage(message);
            if (++ErrorCount > MaxErrors)
            {
                AbortTranslation(ErrorCode.TooManyErrors, producer);
            }
        }

        public static void AbortTranslation(ErrorCode errorCode, IMessageProducer producer)
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
