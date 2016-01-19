﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Compiler.backend;
using Compiler.frontend;
using Compiler.frontend.pascal;
using Compiler.intermediate;
using Compiler.message;
using static Compiler.message.MessageType;

namespace Compiler
{
    public class Pascal
    {
        private Parser _parser;
        private Source _source;
        private ICode _iCode;
        private ISymbolTable _symbolTable;
        private Backend _backend;

        public Pascal(string operation, string filePath, string flags)
        {
            try
            {
                bool intermediate = flags.IndexOf('i') > -1;
                bool xref = flags.IndexOf('x') > -1;

                _source = new Source(new StreamReader(filePath));
                _source.AddMessageListener(new SourceMessageListener());

                _parser = FrontendFactory.CreateParser("pascal", "top-down", _source);
                _parser.AddMessageListener(new ParserMessageListener());

                _backend = BackendFactory.CreateBackend(operation);
                _backend.AddMessageListener(new BackendMessageListener());

                _parser.Parse();
                _source.Close();

                _iCode = _parser.ICode;
                _symbolTable = Parser.SymbolTable;

                _backend.Process(_iCode, _symbolTable);
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
                    new Pascal(operation, path, flags);
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
            "\n{2} seconds total parsing time.\n";

        private class ParserMessageListener : IMessageListener
        {
            public void MessageReceived(Message message)
            {
                MessageType type = message.Type;

                switch (type)
                {
                    case ParserSummary:
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
            public void MessageReceived(Message message)
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
            public void MessageReceived(Message message)
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