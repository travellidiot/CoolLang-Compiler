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
                Scanner scanner = new cool.CoolScanner(source, 2);
                return new TdParser(scanner);
            }
            else {
                throw new Exception("Parser factory: Invalid type '" +
                                    type + "'");
            }
        } 
    }
}