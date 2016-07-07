using System;
using System.Diagnostics;
using System.IO;
using Compiler.backend;
using Compiler.frontend;
using Compiler.frontend.cool;
using Compiler.intermediate.cool.symtab;
using Compiler.message;
using Compiler.utils;
using static Compiler.message.MessageType;

namespace Compiler
{
    public class Program
    {
        public Program(string operation, string filePath, string flags)
        {
            try
            {
                var intermediate = flags.IndexOf('i') > -1;
                var xref = flags.IndexOf('x') > -1;

                var fname = Path.GetFileNameWithoutExtension(filePath);
                var source = new Source(new StreamReader(filePath), fname);
                //_source.AddMessageListener(new SourceMessageListener());
                var sourceMessageListener = new SourceMessageListener(source);

                var parser = FrontendFactory.CreateParser("cool", "top-down", source);
                //_parser.AddMessageListener(new ParserMessageListener());
                var parserMessageListener = new ParserMessageListener(parser);

                var backend = BackendFactory.CreateBackend(operation);
                //_backend.AddMessageListener(new BackendMessageListener());
                var backendMessageListener = new BackendMessageListener(backend);

                var swriter = new StreamWriter(Console.OpenStandardOutput());
                var logger = new LoggerUtil(swriter);

                parser.Parse();
                source.Close();

                var ast = parser.AstRoot;

                Debug.Assert(Parser.ScopeStack.Count == 1);

                var scope = (GlobalSymbolScope)Parser.ScopeStack.Peek();
                //scope.CompleteSymbolType();

                if (xref)
                {
                    logger.LogEmptyLines(5);
                    logger.LogSymbolScope(scope);
                    logger.LogEmptyLines(5);
                }

                backend.Process(ast);
            }
            catch (CoolErrorHandler.FatalErrorException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.StackTrace);
            }
            catch (Exception ex)
            {
                Console.WriteLine("********** Internal translation error. ************");
                Console.WriteLine(ex.StackTrace);
            }
        }

        private static string Flags => "[-ix]";
        private static string Usage =>
            "Usage: Pascal execute|compile " + Flags + " <source file path>";



        static void Main(string[] args)
        {
            try
            {
                var operation = args[0];
                if (!(operation.Equals("compile")
                      || operation.Equals("execute")))
                {
                    throw new Exception();
                }

                var i = 0;
                var flags = "";

                while ((++i < args.Length) && (args[i][0] == '-'))
                {
                    flags += args[i].Substring(1);
                }

                if (i < args.Length)
                {
                    var path = args[i];
                    new Program(operation, path, flags);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                Console.WriteLine(Usage);
            }
        }



        private static string ParserSummaryFormat =>
            "\n{0} source lines." +
            "\n{1} syntax errors." +
            "\n{2} milliseconds total parsing time.\n";

        private static string SyntaxErrorFormat =>
            "\nline {0}, col {1}: \"{2}\":{3}, {4}.\n";

        private class ParserMessageListener : IMessageListener
        {
            public ParserMessageListener(IMessageProducer messageProducer)
            {
                messageProducer.AddListener(this);
            }

            public void MessageReceived(Message message)
            {
                var type = message.Type;

                switch (type)
                {
                    case MessageType.ParserSummary:
                            var bodies = (object[])message.Body;
                            int statementCount = (int)bodies[0];
                            int syntaxErrors = (int)bodies[1];
                            long elapsedTime = (long)bodies[2];

                            Console.WriteLine(ParserSummaryFormat,
                                              statementCount, syntaxErrors,
                                              elapsedTime);
                            break;

                    case MessageType.SyntaxError:
                        var body = (object[])message.Body;
                        var lineNumber = (int) body[0];
                        var position = (int) body[1];
                        var text = (string) body[2];
                        var tokenType = (CoolTokenType) body[3];
                        var errorCode = (string) body[4];
                        Console.WriteLine(SyntaxErrorFormat, lineNumber, position, text, tokenType.CoolType, errorCode);
                        break;
                }
            }
        }

        private static string InterpreterSummaryFormat => "\n{0} statements executed." + "\n{1} runtime errors." + "\n{2} seconds total execution time.\n";

        private static string CompilerSummaryFormat => "\n{0} instructions generated." + "\n{1} seconds total code generation time.\n";


        private class BackendMessageListener : IMessageListener
        {
            public BackendMessageListener(IMessageProducer producer)
            {
                producer.AddListener(this);
            }

            public void MessageReceived(Message message)
            {
                MessageType type = message.Type;

                switch (type)
                {
                    case InterpreterSummary:
                    {
                        var body = (object[]) message.Body;
                        int executionCount = (int) body[0];
                        int runtimeErrors = (int) body[1];
                        long elapsedTime = (long) body[2];

                        Console.WriteLine(InterpreterSummaryFormat, executionCount, runtimeErrors, elapsedTime);
                        break;
                    }

                    case CompilerSummary:
                    {
                        var body = (object[]) message.Body;
                        int instructionCount = (int) body[0];
                        long elapsedTime = (long) body[1];

                        Console.WriteLine(CompilerSummaryFormat, instructionCount, elapsedTime);
                        break;
                    }
                }
            }
        }


        private static string SourceLineFormat => "%03d %s";

        private class SourceMessageListener : IMessageListener
        {
            public SourceMessageListener(IMessageProducer producer)
            {
                producer.AddListener(this);
            }

            public void MessageReceived(Message message)
            {
                var type = message.Type;
                var bodies = (object[]) message.Body;

                switch (type)
                {
                    case SourceLine:
                    {
                        var lineNumber = (int) bodies[0];
                        var lineText = (string) bodies[1];
                        Console.WriteLine(SourceLineFormat, lineNumber, lineText);
                        break;
                    }
                }
            }
        }
    }
}
