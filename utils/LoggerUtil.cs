using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.frontend;
using Compiler.frontend.cool;
using Compiler.intermediate;
using Compiler.intermediate.coolsymtab;

namespace Compiler.utils
{
    internal class LoggerUtil
    {
        private readonly StreamWriter _logger = new StreamWriter(Console.OpenStandardOutput());
        
        public LoggerUtil()
        {
            _logger.AutoFlush = true;
        }
        public LoggerUtil(StreamWriter swriter)
        {
            _logger = swriter;
            _logger.AutoFlush = true;
        }

        public void LogEmptyLines(int num)
        {
            for (int i = 0; i < num; ++i)
            {
                _logger.WriteLine();
            }
        }

        public void Log(string message)
        {
            _logger.Write(message);
        }

        public void LogLine(string message)
        {
            _logger.WriteLine(message);
        }

        public void LogToken(Token token)
        {
            CoolTokenType tokenType = token.Type as CoolTokenType;
            string t = tokenType?.Text;

            _logger.WriteLine("{0, -20}\t{1, -20}\tline: {2}", token.Text, t, token.LineNumber);
        }

        private string FTypeToString(List<IType> signature)
        {
            var builder = new StringBuilder();
            var prefix = "";
            foreach (var t in signature)
            {
                builder.Append(prefix);
                builder.Append(t.TypeName);
                if (prefix == "")
                    prefix = "->";
            }

            return builder.ToString();
        }

        public void LogSymbolScope(SymbolScope scope)
        {
            var queue = new Queue<SymbolScope>();
            queue.Enqueue(scope);

            while (queue.Count > 0)
            {
                var symScope = queue.Dequeue();
                _logger.WriteLine("=====  Cross-Reference {0} Table ========", symScope.SymName);
                _logger.WriteLine("{0, -20}\t{1, -20}\t{2, -20}", "ID", "Type", "TypeFlag");
                _logger.WriteLine("{0, -20}\t{1, -20}\t{2, -20}", "--", "----", "--------");

                var entries = symScope.Symbols;
                foreach (var entry in entries)
                {
                    if (entry.Value.GetType() == typeof (ClassSymbolScope) || entry.Value.GetType() == typeof(SymbolScope))
                        queue.Enqueue(entry.Value as SymbolScope);

                    var key = entry.Key;
                    var symType = entry.Value.SymType;
                    

                    var classType = symType as ClassSymbolScope;
                    if (classType != null)
                        _logger.WriteLine("{0, -20}\t{1, -20}\t{2, -20}", key, classType.SymName, classType.TypeName);
                    else
                    {
                        var methodType = entry.Value as MethodSymbolScope;
                        if (methodType != null)
                        {
                            var signature = FTypeToString(methodType.TypeSignature);
                            _logger.WriteLine("{0, -20}\t{1, -20}\t{2, -20}", key, signature, "--------");
                        }
                    }
                }

                _logger.WriteLine("\n");
            }
        }
    }
}
