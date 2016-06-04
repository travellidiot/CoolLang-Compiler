using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Compiler.backend;
using Compiler.frontend;
using Compiler.intermediate;
using Compiler.message;
using Compiler.utils;
using static Compiler.message.MessageType;

namespace Compiler
{
    public class Program
    {
        private Parser _parser;
        private Source _source;
        private IAst _ast;
        private ISymTabStack _symTabStack;
        private Backend _backend;
        private LoggerUtil _logger;

        public Program(string operation, string filePath, string flags)
        {
            try
            {
                bool intermediate = flags.IndexOf('i') > -1;
                bool xref = flags.IndexOf('x') > -1;

                _source = new Source(new StreamReader(filePath));
                //_source.AddMessageListener(new SourceMessageListener());
                var sourceMessageListener = new SourceMessageListener(_source);

                _parser = FrontendFactory.CreateParser("cool", "top-down", _source);
                //_parser.AddMessageListener(new ParserMessageListener());
                var parserMessageListener = new ParserMessageListener(_parser);

                _backend = BackendFactory.CreateBackend(operation);
                //_backend.AddMessageListener(new BackendMessageListener());
                var backendMessageListener = new BackendMessageListener(_backend);

                StreamWriter swriter = new StreamWriter(Console.OpenStandardOutput());
                _logger = new LoggerUtil(swriter);

                _parser.Parse();
                _source.Close();

                _ast = _parser.AstRoot;
                _symTabStack = Parser.SymTabStack;

                if (xref)
                {
                    _logger.LogEmptyLines(5);
                    _logger.LogSymTabStack(_symTabStack);
                    _logger.LogEmptyLines(5);
                }

                _backend.Process(_ast, _symTabStack);
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
                string operation = args[0];
                if (!(operation.Equals("compile")
                      || operation.Equals("execute")))
                {
                    throw new Exception();
                }

                int i = 0;
                string flags = "";

                while ((++i < args.Length) && (args[i][0] == '-'))
                {
                    flags += args[i].Substring(1);
                }

                if (i < args.Length)
                {
                    string path = args[i];
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

        private class ParserMessageListener : IMessageListener
        {
            public ParserMessageListener(IMessageProducer messageProducer)
            {
                messageProducer.MessageHandler += MessageReceived;
            }

            public void MessageReceived(object sender, Message message)
            {
                MessageType type = message.Type;

                switch (type)
                {
                    case MessageType.ParserSummary:
                        {
                            var bodies = (object[])message.Body;
                            int statementCount = (int)bodies[0];
                            int syntaxErrors = (int)bodies[1];
                            long elapsedTime = (long)bodies[2];

                            Console.WriteLine(ParserSummaryFormat,
                                              statementCount, syntaxErrors,
                                              elapsedTime);
                            break;
                        }
                }
            }
        }

        private static string InterpreterSummaryFormat =>
            "\n{0} statements executed." +
            "\n{1} runtime errors." +
            "\n{2} seconds total execution time.\n";

        private static string CompilerSummaryFormat =>
            "\n{0} instructions generated." +
            "\n{1} seconds total code generation time.\n";


        private class BackendMessageListener : IMessageListener
        {
            public BackendMessageListener(IMessageProducer producer)
            {
                producer.MessageHandler += MessageReceived;
            }
            public void MessageReceived(object sender, Message message)
            {
                MessageType type = message.Type;

                switch (type)
                {
                    case InterpreterSummary:
                        {
                            var body = (object[])message.Body;
                            int executionCount = (int)body[0];
                            int runtimeErrors = (int)body[1];
                            long elapsedTime = (long)body[2];

                            Console.WriteLine(InterpreterSummaryFormat,
                                              executionCount, runtimeErrors,
                                              elapsedTime);
                            break;
                        }

                    case CompilerSummary:
                        {
                            var body = (object[])message.Body;
                            int instructionCount = (int)body[0];
                            long elapsedTime = (long)body[1];

                            Console.WriteLine(CompilerSummaryFormat,
                                              instructionCount, elapsedTime);
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
                producer.MessageHandler += MessageReceived;
            }
            public void MessageReceived(object sender, Message message)
            {
                var type = message.Type;
                var bodies = (object[]) message.Body;

                switch (type)
                {
                    case SourceLine: {
                        int lineNumber = (int) bodies[0];
                        string lineText = (string) bodies[1];
                        Console.WriteLine(String.Format(SourceLineFormat,
                            lineNumber, lineText));
                        break;
                    }
                }
            }
        }
    }
}
