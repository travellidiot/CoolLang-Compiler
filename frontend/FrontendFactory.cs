using System;
using Compiler.frontend.cool;
using Compiler.frontend.pascal;

namespace Compiler.frontend
{
    public class FrontendFactory
    {
        public static Parser CreateParser(string language, string type, Source source)
        {
            if (language.Equals("pascal") &&
            type.Equals("top-down"))
            {
                Scanner scanner = new PascalScanner(source);
                return new PascalParserTopDown(scanner);
            }
            else if (language.Equals("cool") && type.Equals("top-down"))
            {
                Scanner scanner = new CoolScanner(source);
                return new CoolParserTopDown(scanner);
            }
            else {
                throw new Exception("Parser factory: Invalid type '" +
                                    type + "'");
            }
        } 
    }
}