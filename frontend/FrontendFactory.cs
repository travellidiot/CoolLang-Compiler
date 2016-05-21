using System;
using Compiler.frontend.cool;

namespace Compiler.frontend
{
    public class FrontendFactory
    {
        public static Parser CreateParser(string language, string type, Source source)
        {
            if (language.Equals("cool") && type.Equals("top-down"))
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