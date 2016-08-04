using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.frontend;
using Compiler.frontend.cool;
using Compiler.intermediate;
using Compiler.intermediate.cool.symtab;

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
            var tokenType = token.Type as CoolTokenType;
            var t = tokenType?.Text;

            _logger.WriteLine("{0, -20}\t{1, -20}\tline: {2}", token.Text, t, token.LineNumber);
        }

        private static string FTypeToString(IEnumerable<ISymbol> signature)
        {
            var builder = new StringBuilder();
            var prefix = "";
            foreach (var t in signature)
            {
                builder.Append(prefix);
                builder.Append(t.SymName);
                if (prefix == "")
                    prefix = "->";
            }

            return builder.ToString();
        }

        public void LogSymbolScope(SymbolScope scope)
        {

        }
    }
}
