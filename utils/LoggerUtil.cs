using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.frontend;
using Compiler.frontend.cool;
using Compiler.intermediate;

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

        public void LogSymTabStack(IScopeStack scopeStack)
        {
            var table = scopeStack.LocalScope;
            var entries = table.SortedEntries();

            _logger.WriteLine("=====  Cross-Reference Table ========");
            _logger.WriteLine("{0, -20}\t{1, -20}", "Object ID", "Line Numbers");
            _logger.WriteLine("{0, -20}\t{1, -20}", "---------", "------------");

            foreach (var entry in entries)
            {
                var lineNumbers = entry.LineNumbers;
                if (lineNumbers != null)
                {
                    _logger.Write("{0, -20}\t", entry.Name);
                    foreach (var num in lineNumbers)
                    {
                        _logger.Write(" {0}", num);
                    }
                    _logger.WriteLine();
                }
            }
        }
    }
}
