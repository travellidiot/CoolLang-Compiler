using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.frontend;
using Compiler.frontend.cool;

namespace Compiler.utils
{
    class LoggerUtil
    {
        public static void LogToken(Token token)
        {
            CoolTokenType tokenType = token.Type as CoolTokenType;

            Console.Write(token.Text);
            Console.Write(token.Text.Length < 8 ? "\t\t" : "\t");

            string t = tokenType?.CoolType.ToString();
            Console.Write(t);
            Console.Write(t != null && t.Length < 8 ? "\t\t" : "\t");
            Console.WriteLine("line: {0}", token.LineNumber);
        }
    }
}
