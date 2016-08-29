using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Compiler.backend;
using Compiler.frontend;
using Compiler.frontend.cool;
using Compiler.intermediate.cool.symtab;
using Compiler.message;
using Compiler.message.cool;
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
                Console.WriteLine("{0}", filePath);
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
                var gScope = GlobalSymbolScope.Instance;

                var symbolBuilder = new SymbolBuilder(gScope);
                var sbMessageListener = new SemantListener(symbolBuilder);
                ast.Root.Accept(symbolBuilder);

                var semantChecker = new SemantChecker(gScope);
                var scMessageListener = new SemantListener(semantChecker);
                ast.Root.Accept(semantChecker);

                backend.Process(ast, gScope);
            }
            catch (ErrorHandler.FatalErrorException ex)
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
            "Usage: Program execute|compile " + Flags + " <source file path>";

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
                    var dirInfo  = new DirectoryInfo(args[i]);
                    Console.WriteLine("{0}", dirInfo.ToString()); 
                    foreach (var program in dirInfo.GetFiles()
                                                   .Select(fi => Path.Combine(dirInfo.ToString(), fi.Name))
                                                   .Select(path => new Program(operation, path, flags)))
                    {
                    }
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
    }
}
