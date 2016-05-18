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
    class LoggerUtil
    {
        private static readonly StreamWriter Logger = new StreamWriter(Console.OpenStandardOutput());


        public static void LogEmptyLines(int num)
        {
            for (int i = 0; i < num; ++i)
            {
                Logger.WriteLine();
            }
        }

        public static void LogToken(Token token)
        {
            Logger.AutoFlush = true;

            CoolTokenType tokenType = token.Type as CoolTokenType;

            Logger.Write(token.Text);
            Logger.Write(token.Text.Length < 8 ? "\t\t" : "\t");

            string t = tokenType?.Text;
            Logger.Write(t);
            Logger.Write(t != null && t.Length < 8 ? "\t\t" : "\t");
            Logger.WriteLine("line: {0}", token.LineNumber);
        }

        public static void LogSymTabStack(ISymTabStack symTabStack)
        {
            Logger.AutoFlush = true;

            var table = symTabStack.LocalSymTab;
            var entries = table.SortedEntries();

            Logger.WriteLine("=====  Cross-Reference Table ========");
            Logger.WriteLine("{0, -20}\t{1, -20}", "Object ID", "Line Numbers");
            Logger.WriteLine("{0, -20}\t{1, -20}", "---------", "------------");

            foreach (var entry in entries)
            {
                var lineNumbers = entry.LineNumbers;
                if (lineNumbers != null)
                {
                    Logger.Write("{0, -20}\t", entry.Name);
                    foreach (var num in lineNumbers)
                    {
                        Logger.Write(" {0}", num);
                    }
                    Logger.WriteLine();
                }
            }
        }
    }
}
